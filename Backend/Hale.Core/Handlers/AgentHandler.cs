namespace Hale.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Hale.Core.Config;
    using Hale.Core.Data.Contexts;
    using Hale.Core.Data.Entities.Modules;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Lib;
    using Hale.Lib.JsonRpc;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;
    using Hale.Lib.Utilities;
    using Newtonsoft.Json.Linq;
    using NLog;
    using Piksel.Nemesis;
    using Piksel.Nemesis.Security;
    using EModule = Hale.Core.Data.Entities.Modules.Module;

    internal class AgentHandler : IDisposable
    {
        private readonly ILogger log;
        private readonly HaleDBContext db = new HaleDBContext();
        private readonly CoreConfig.AgentSection agentConfig;
        private readonly Dictionary<Guid, int> hostGuidsToIds = new Dictionary<Guid, int>();
        private readonly string agentKeyStorePath;
        private readonly string coreKeyFilePath;

        private NemesisHub nemesis;
        private XMLFileKeyStore xmlFileKeyStore;

        public AgentHandler()
        {
            try
            {
                var agentConfig = ServiceProvider.GetServiceCritical<CoreConfig>().Agent;

                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                this.log = LogManager.GetLogger("Hale.Core.AgentHandler");

                this.agentConfig = agentConfig;

                if (agentConfig.UseEncryption)
                {
                    string keyRepoPath = Path.Combine(appDataPath, "Hale", "Core");

                    this.coreKeyFilePath = Path.Combine(keyRepoPath, "core-keys.xml");
                    this.agentKeyStorePath = Path.Combine(keyRepoPath, "HostKeys");

                    this.LoadXmlFileKeyStore();
                    this.GenerateRsaKeys();
                }

                this.LoadNemesisClient(agentConfig.UseEncryption);
            }
            catch (Exception x)
            {
                this.log.Error(x, $"Failed to initialize Agent Handler: {x.Message}");
            }
        }

        internal RSAKey PublicKey
        {
            get
            {
                return this.nemesis.KeyStore.PublicKey;
            }
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        public void LoadKeys()
        {
            this.hostGuidsToIds.Clear();

            var hosts = this.db.Nodes.ToList();
            hosts.ForEach(host =>
            {
                // Todo: Handle concurrent dictionary better -NM
                this.nemesis.NodesPublicKeys.TryAdd(host.Guid, new RSAKey { Key = host.RsaKey });
                this.hostGuidsToIds.Add(host.Guid, host.Id);
            });
        }

        public void GenerateRsaKeys()
        {
            var hosts = this.db.Nodes.ToList();

            EnvironmentConfig.AffirmPath(this.agentKeyStorePath);
            hosts.ForEach(host =>
            {
                this.CreateKeystore(this.agentKeyStorePath, host);
            });
        }

        private static JsonRpcResponse FetchApplicationErrorResponse(JsonRpcRequest req, Exception x)
        {
            return new JsonRpcResponse(req)
            {
                Error = new JsonRpcError()
                {
                    Code = JsonRpcErrorCode.ApplicationError,
                    Message = x.Message
                }
            };
        }

        private void LoadXmlFileKeyStore()
        {
            this.xmlFileKeyStore = new XMLFileKeyStore(this.coreKeyFilePath, new RSA(), true);

            if (!this.xmlFileKeyStore.Available)
            {
                this.xmlFileKeyStore.Save();
            }
        }

        private void LoadNemesisClient(bool useEncryption)
        {
            this.nemesis = new NemesisHub(
                new IPEndPoint(this.agentConfig.Ip, this.agentConfig.SendPort),
                new IPEndPoint(this.agentConfig.Ip, this.agentConfig.ReceivePort));

            // Allow new nodes to connect -NM 2016-11-26
            this.nemesis.AllowUnknownGuid = true;

            if (useEncryption)
            {
                this.nemesis.EnableEncryption(this.xmlFileKeyStore);
            }

            this.LoadKeys();

            this.nemesis.CommandReceived += this.NemesisCommandReceived;
        }

        private void NemesisCommandReceived(object sender, CommandReceivedEventArgs e)
        {
            JsonRpcResponse response;

            try
            {
                var req = JsonRpcRequest.FromJsonString(e.Command);
                this.log.Debug("Got message of type \"{0}\"... ", req.Method);
                response = this.ExecuteRequest(e, req);
            }
            catch (Exception x)
            {
                this.log.Warn(x, $"Got invalid RPC command from Node! Error: {x.Message}");
                response = new JsonRpcResponse()
                {
                    Error = new JsonRpcError()
                    {
                        Code = JsonRpcErrorCode.ParseNotWellFormed,
                        Message = x.Message,
                        Data = x
                    }
                };
            }

            var serialized = JsonRpcDefaults.Encoding.GetString(response.Serialize());
            e.ResultSource.SetResult(serialized);
        }

        private JsonRpcResponse ExecuteRequest(CommandReceivedEventArgs e, JsonRpcRequest request)
        {
            JsonRpcResponse response = new JsonRpcResponse(request);

            if (request.Method == "heartbeat")
            {
                response = this.RpcHeartbeat(e.NodeId, request);
            }
            else if (request.Method == "getAgentConfig")
            {
                response = this.RpcGetAgentConfig(e.NodeId, request);
            }
            else if (request.Method == "uploadResults")
            {
                response = this.RpcUploadResults(e.NodeId, request);
            }
            else if (request.Method == "identifyAgent")
            {
                response = this.RpcIdentifyAgent(e.NodeId, request);
            }
            else
            {
                response.Error = new JsonRpcError()
                {
                    Code = JsonRpcErrorCode.ServerRequestedMethodNotFound,
                    Message = "Requested method not found."
                };
            }

            return response;
        }

        private JsonRpcResponse RpcUploadResults(Guid nodeId, JsonRpcRequest req)
        {
            using (var db = new HaleDBContext())
            {
                if (!this.hostGuidsToIds.ContainsKey(nodeId))
                {
                    return new JsonRpcResponse(req)
                    {
                        Error = new JsonRpcError()
                        {
                            Code = JsonRpcErrorCode.ServerInvalidMethodParameters,
                            Message = "Unknown GUID"
                        }
                    };
                }

                try
                {
                    var received = new List<Guid>();

                    var firstParam = (JToken)req.Params[0];
                    var records = firstParam.ToObject<ResultRecordChunk>();

                    this.log.Info($"Got {records.Count} records from node {nodeId.ToString()}.");
                    foreach (var kvpRecord in records)
                    {
                        Guid guid = kvpRecord.Key;
                        ModuleResultRecord record = ((JObject)kvpRecord.Value).ToObject<ModuleResultRecord>();

                        this.log.Debug($"{guid.ToString()} {record.Module}[{record.FunctionType}]{record.Function}:{record.Results.Count}");

                        if (record.FunctionType == ModuleFunctionType.Check)
                        {
                            foreach (var kvpResult in record.Results)
                            {
                                var target = kvpResult.Key;
                                var result = ((JObject)kvpResult.Value).ToObject<CheckResult>();
                                this.log.Debug($" - {target} => {result.RawValues.Count} raws, S:{(result.RanSuccessfully ? 1 : 0)} W:{(result.Warning ? 1 : 0)} C:{(result.Critical ? 1 : 0)} {result.Message}");

                                Result r = this.ResolveToResultEntity(target, record, result, nodeId);
                                db.Results.Add(r);

                                foreach (var rawvalue in result.RawValues)
                                {
                                    db.CheckRecords.Add(new CheckRecord()
                                    {
                                        Result = r,
                                        Key = record.Module.Identifier + "." + rawvalue.DataType,
                                        Value = rawvalue.Value
                                    });
                                }

                                db.SaveChanges();
                            }
                        }
                        else if (record.FunctionType == ModuleFunctionType.Info)
                        {
                            foreach (var kvpResult in record.Results)
                            {
                                var target = kvpResult.Key;
                                var result = ((JObject)kvpResult.Value).ToObject<InfoResult>();
                                this.log.Debug($" - ({target}) => {result.ItemsAsString()}");

                                Result r = this.ResolveToResultEntity(target, record, result, nodeId);
                                db.Results.Add(r);

                                foreach (var item in result.Items)
                                {
                                    this.db.InfoRecords.Add(new InfoRecord()
                                    {
                                        Result = r,
                                        Key = record.Module.Identifier + "." + item.Key,
                                        Value = item.Value
                                    });
                                }

                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            this.log.Warn($"Module function type {record.FunctionType} is not supported!");
                        }

                        received.Add(guid);
                    }

                    return new JsonRpcResponse(req)
                    {
                        Result = received.ToArray()
                    };
                }
                catch (Exception x)
                {
                    this.log.Warn(x, "Error handling upload from agent: " + x.Message);
                    return FetchApplicationErrorResponse(req, x);
                }
            }
        }

        private Result ResolveToResultEntity(string target, IModuleResultRecord record, ModuleResult moduleResult, Guid nodeId)
        {
            Node host = this.ResolveHost(nodeId);
            var module = this.ResolveModule(record);
            Function function = this.ResolveModuleFunction(record, module);
            Result result = this.ConvertToResult(record, moduleResult, host, function, module, target);
            return result;
        }

        private EModule ResolveModule(IModuleResultRecord record)
        {
            var v = record.Module.Version;
            var module = this.db.Modules.SingleOrDefault(m =>
                m.Major == v.Major &&
                m.Minor == v.Minor &&
                m.Revision == v.Patch &&
                m.Identifier == record.Module.Identifier);
            if (module == null)
            {
                module = new EModule()
                {
                    Version = v,
                    Identifier = record.Module.Identifier
                };
                this.db.Modules.Add(module);
                this.log.Warn($"Added missing module '{record.Module.ToString()}' to database.");
                this.db.SaveChanges();
            }

            return module;
        }

        private Function ResolveModuleFunction(IModuleResultRecord record, EModule module)
        {
            var func = this.db.Functions.SingleOrDefault(f =>
                f.Name == record.Function &&
                f.Module.Id == module.Id &&
                f.Type == record.FunctionType);

            // Todo: Decide if we really want to add all module functions that are missing to the database here -NM 2016-08-13
            if (func == null)
            {
                func = this.db.Functions.Add(new Function()
                {
                    Name = record.Function,
                    Module = module,
                    Type = record.FunctionType
                }).Entity;
                this.log.Warn($"Added missing module function '{record.Module.ToString()}[{record.FunctionType}]{record.Function}' to database.");

                this.db.SaveChanges();
            }

            return func;
        }

        private Result ConvertToResult(IModuleResultRecord record, ModuleResult moduleResult, Node host, Function function, EModule module, string target)
        {
            Result result = new Result()
            {
                HostId = host.Id,
                FunctionId = function.Id,
                ModuleId = module.Id,
                Exception = !moduleResult.RanSuccessfully ? moduleResult.ExecutionException.Message : null,

                // Note: CheckException does not contain the same fields as the Core Entity.
                ExecutionTime = record.CompletionTime,
                Message = moduleResult.Message,
                Target = target
            };

            if (moduleResult is CheckResult checkResult)
            {
                result.AboveCritical = checkResult.Critical;
                result.AboveWarning = checkResult.Warning;
            }

            return result;
        }

        private Node ResolveHost(Guid guid)
        {
            return this.db.Nodes.SingleOrDefault(h => h.Guid == guid);
        }

        private int GetCheckResultType(CheckResult result)
        {
            if (result.RanSuccessfully)
            {
                if (result.Critical)
                {
                    return (int)Status.Error;
                }
                else if (result.Warning)
                {
                    return (int)Status.Warning;
                }
                else
                {
                    return (int)Status.Ok;
                }
            }
            else
            {
                return (int)Status.Error;
            }
        }

        private JsonRpcResponse RpcGetAgentConfig(Guid nodeId, JsonRpcRequest req)
        {
            throw new NotImplementedException();
        }

        private JsonRpcResponse RpcCheckResults(Guid nodeId, JsonRpcRequest req)
        {
            throw new NotImplementedException();
        }

        private JsonRpcResponse RpcIdentifyAgent(Guid nodeId, JsonRpcRequest req)
        {
            var firstParam = (JToken)req.Params[0];
            var agentId = firstParam.ToObject<AgentIdentification>();

            var sbNics = new StringBuilder();

            this.log.Info($"Got agent identification for node {nodeId}.");

            this.log.Debug("Hostname: " + agentId.Hostname);
            this.log.Debug("Operating system: " + agentId.OperatingSystem);
            this.log.Debug("Network interfaces: ");

            foreach (var nic in agentId.NetworkInterfaces)
            {
                this.log.Debug(" - " + nic.Name);
                sbNics.AppendLine(" - " + nic.Name);
                this.log.Debug("   " + nic.PhysicalAddress);
                sbNics.AppendLine("   " + nic.PhysicalAddress);
                this.log.Debug("   " + string.Join(", ", nic.Addresses));
                sbNics.AppendLine("   " + string.Join(", ", nic.Addresses));
            }

            this.db.Nodes.Add(new Node()
            {
                Configured = false,
                HostName = agentId.Hostname,
                Guid = nodeId,
                HardwareSummary = agentId.HardwareSummary,
                NicSummary = sbNics.ToString(),
                OperatingSystem = agentId.OperatingSystem,
                LastConnected = DateTime.UtcNow,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            });
            this.db.SaveChanges();

            // Reload GUIDs/keys from database to permit the new node to be identified -NM 2016-11-27
            this.LoadKeys();

            this.log.Info($"Unconfigured node {nodeId} added to database.");

            return new JsonRpcResponse(req)
            {
                Result = "OK"
            };
        }

        private JsonRpcResponse RpcHeartbeat(Guid nodeId, JsonRpcRequest req)
        {
            if (!this.hostGuidsToIds.ContainsKey(nodeId))
            {
                return new JsonRpcResponse(req)
                {
                    /*
                    Error = new JsonRpcError()
                    {
                        Code = JsonRpcErrorCode.ServerInvalidMethodParameters,
                        Message = "Unknown GUID"
                    }
                    */
                    Result = "UnknownGUID"
                };
            }

            Node host = this.db.Nodes.Find(this.hostGuidsToIds[nodeId]);
            host.Status = (int)Status.Ok;

            this.db.SaveChanges();

            return new JsonRpcResponse(req)
            {
                Result = "OK"
            };
        }

        private void CreateKeystore(string hostKeysPath, Node host)
        {
            // Hack: Stores agent keys as XML as well as in database for easier development @fixme @security -NM
            // Todo: Write a proper keystore with database as sole backend
            var xfks = new XMLFileKeyStore(Path.Combine(hostKeysPath, host.Guid + ".xml"), RSA.Default, true);

            // Only write to disk and database if new keys has been generated. -NM
            if (!xfks.Available)
            {
                xfks.Save();
            }

            // Hack: Since we dont want to generate new keys even if the database has been cleaned right now -NM
            if (host.RsaKey == null)
            {
                var thost = this.db.Nodes.Attach(host);
                thost.Entity.RsaKey = xfks.PrivateKey.Key;

                this.db.SaveChanges();
            }
        }
    }
}

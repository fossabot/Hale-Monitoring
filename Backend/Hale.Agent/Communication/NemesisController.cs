namespace Hale.Agent.Communication
{
    using System;
    using Hale.Agent.Config;
    using Hale.Lib;
    using Hale.Lib.JsonRpc;
    using Hale.Lib.Modules.Results;
    using Hale.Lib.Utilities;
    using Newtonsoft.Json.Linq;
    using NLog;
    using Piksel.Nemesis;
    using Piksel.Nemesis.Security;

    internal class NemesisController
    {
        private readonly ILogger log = LogManager.GetLogger("NemesisController");

        private readonly NemesisNode node;

        public NemesisController()
        {
            var env = ServiceProvider.GetService<EnvironmentConfig>();

            this.log.Debug("Loading nemesis config file \"{0}\"...", env.NemesisConfigFile);
            var config = NemesisConfig.LoadFromFile(env.NemesisConfigFile);

            this.log.Debug($"Host: {config.Hostname}, Send port: {config.SendPort}, Receive port: {config.ReceivePort}, Encryption: {config.UseEncryption}, GUID: {config.Id}");

            this.node = new NemesisNode(config.Id, new int[] { config.ReceivePort, config.SendPort }, config.Hostname, false);
            if (config.UseEncryption && env.NemesisKeyFile != string.Empty)
            {
                var ke = new RSA();

                this.log.Debug("Loading encryption keys from \"{0}\"...", env.NemesisKeyFile);
                var keystore = new XMLFileKeyStore(env.NemesisKeyFile, ke);

                var coreKeystore = new XMLFileKeyStore(env.NemesisKeyFile.Replace("agent", "core"), ke);
                this.node.HubPublicKey = coreKeystore.PublicKey;

                this.node.EnableEncryption(keystore);

                this.log.Debug("Encryption is enabled.");
            }

            this.node.Connect();
        }

        public void Stop()
        {
            // TODO: Remove or implement me? -NM 2017-08-16
        }

        public string RetrieveString(string command, params object[] parameters)
        {
            var req = new JsonRpcRequest()
            {
                Method = command
            };

            if (parameters.Length > 0)
            {
                req.Params = parameters;
            }

            try
            {
                var serialized = JsonRpcDefaults.Encoding.GetString(req.Serialize());
                var respTask = this.node.SendCommand(serialized);
                var response = JsonRpcResponse.FromJsonString(respTask.Result); // Blocking!
                if (response != null && response.Error == null)
                {
                    var result = (string)response.Result;
                    if (result != null)
                    {
                        this.log.Debug($"Got a {result.Length} character response string.");
                    }

                    return result;
                }
                else
                {
                    if (response == null)
                    {
                        this.log.Error("Got empty response!");
                    }
                    else
                    {
                        this.log.Error(response.Error?.Data, $"Got JSONRPC Error: {response.Error.Code}, Message: {response.Error.Message}");
                    }

                    return string.Empty;
                }
            }
            catch (Exception x)
            {
                this.log.Error("Error when retrieving string: {0}", x.Message);
                return string.Empty;
            }
        }

        public string SendCommand(string command)
        {
            return this.RetrieveString(command, new object[0]);
        }

        public Guid[] UploadResults(ResultRecordChunk records)
        {
            if (records.Count < 1)
            {
                return new Guid[0];
            }

            var req = new JsonRpcRequest()
            {
                Method = "uploadResults",
                Params = new object[] { records }
            };
            try
            {
                this.log.Info($"Uploading {records.Count} result records to Core...");
                var serialized = JsonRpcDefaults.Encoding.GetString(req.Serialize());
                var respTask = this.node.SendCommand(serialized);
                var response = JsonRpcResponse.FromJsonString(respTask.Result); // Blocking!
                if (response == null)
                {
                    this.log.Error("Error uploading records: Got an invalid response from Hub");
                    return new Guid[0];
                }
                else if (response.Error != null)
                {
                    this.log.Warn($"Error uploading records: {response.Error.Message} ({response.Error.Code})");
                    return new Guid[0];
                }
                else
                {
                    var result = ((JToken)response.Result).ToObject<Guid[]>();
                    this.log.Info($"Uploaded {result.Length} result records successfully.");
                    return result;
                }
            }
            catch (Exception x)
            {
                this.log.Warn($"Error uploading records: {x.Message}");
                return new Guid[0];
            }
        }
    }
}

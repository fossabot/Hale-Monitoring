using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hale.Agent.Config;
using Piksel.Nemesis;
using Piksel.Nemesis.Security;
using NLog;
using System.ComponentModel;
using System.Threading;
using Newtonsoft.Json.Linq;
using Hale.Agent.Core;
using Hale.Lib.JsonRpc;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules;
using Hale.Lib.Utilities;
using Hale.Lib;

namespace Hale.Agent.Communication
{
    class NemesisController
    {
        ILogger _log = LogManager.GetLogger("NemesisController");

        private readonly NemesisNode _node;

        public NemesisController()
        {
            var env = ServiceProvider.GetService<EnvironmentConfig>();

            _log.Debug("Loading nemesis config file \"{0}\"...", env.NemesisConfigFile);
            var config = NemesisConfig.LoadFromFile(env.NemesisConfigFile);

            _log.Debug("Host: {0}, Send port: {1}, Receive port: {4}, Encryption: {2}, GUID: {3}", 
                config.Hostname, config.SendPort, config.UseEncryption, config.Id, config.ReceivePort);

            _node = new NemesisNode(config.Id, new int[] { config.ReceivePort, config.SendPort }, config.Hostname, false);
            if (config.UseEncryption && string.Empty != env.NemesisKeyFile) {

                var ke = new RSA();

                _log.Debug("Loading encryption keys from \"{0}\"...", env.NemesisKeyFile);
                var keystore = new XMLFileKeyStore(env.NemesisKeyFile, ke);

                var coreKeystore = new XMLFileKeyStore(env.NemesisKeyFile.Replace("agent", "core"), ke);
                _node.HubPublicKey = coreKeystore.PublicKey;

                _node.EnableEncryption(keystore);

                _log.Debug("Encryption is enabled.");
            }

            _node.Connect();

            //_log.Debug("Starting hearbeat worker thread...");
            //heartbeatworker = new NemesisHeartbeatWorker(config, node);
            //heartbeatworker.Start();

            
        }

        public void Stop()
        {

        }

        public string RetrieveString(string command, params object[] parameters)
        {
            var req = new JsonRpcRequest()
            {
                Method = command
            };

            if (parameters.Length > 0)
                req.Params = parameters;

            try
            {
                var serialized = JsonRpcDefaults.Encoding.GetString(req.Serialize());
                var respTask = _node.SendCommand(serialized);
                var response = JsonRpcResponse.FromJsonString(respTask.Result); // Blocking!
                if (response != null && response.Error == null)
                {
                    var result = (string)response.Result;
                    if(result != null)
                        _log.Debug("Got a {0} character response string.", result.Length);
                    return result;
                }
                else
                {
                    if(response == null)
                    {
                        _log.Error("Got empty response!");
                    }
                    else
                    {
                        _log.Error(response.Error?.Data, $"Got JSONRPC Error: {response.Error.Code}, Message: {response.Error.Message}");
                    }
                    
                    return string.Empty;
                }
            }
            catch (Exception x)
            {
                _log.Error("Error when retrieving string: {0}", x.Message);
                return "";
            }
        }

        public string SendCommand(string command)
        {
            return RetrieveString(command, new object[0]);
        }

        public Guid[] UploadResults(ResultRecordChunk records)
        {
            if (records.Count < 1) return new Guid[0];
            var req = new JsonRpcRequest()
            {
                Method = "uploadResults",
                Params = new object[] { records }
            };
            try
            {
                _log.Info($"Uploading {records.Count} result records to Core...");
                var serialized = JsonRpcDefaults.Encoding.GetString(req.Serialize());
                var respTask = _node.SendCommand(serialized);
                var response = JsonRpcResponse.FromJsonString(respTask.Result); // Blocking!
                if (response.Error != null)
                {
                    _log.Warn($"Error uploading records: {response.Error.Message} ({response.Error.Code})");
                    return new Guid[0];
                }
                else
                {
                    var result = ((JToken)response.Result).ToObject<Guid[]>();
                    _log.Info($"Uploaded {result.Length} result records successfully.");
                    return result;
                }
            }
            catch (Exception x)
            {
                _log.Warn($"Error uploading records: {x.Message}");
                return new Guid[0];
            }
        }

        
    }

 
}

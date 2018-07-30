namespace Hale.Agent.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Hale.Agent.Config;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;
    using Hale.Lib.Utilities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using NLog;

    // Todo: Rename class @todo -NM
    internal class ResultStorage : IResultStorage, IDisposable
    {
        private readonly ILogger log = LogManager.GetLogger("ResultStorage");

        private string resultsPath;

        private Queue<IModuleResultRecord> records = new Queue<IModuleResultRecord>();

        public ResultStorage()
        {
            this.resultsPath = ServiceProvider.GetService<EnvironmentConfig>().ResultsPath;
        }

        public void Dispose()
            => this.Persist();

        public void StoreResult(IModuleResultRecord record)
            => this.records.Enqueue(record);

        public void Persist()
        {
            var encoding = new UTF8Encoding(false);

            var js = new Newtonsoft.Json.JsonSerializer();
            js.Converters.Add(new VersionConverter());

            while (this.records.Count > 0)
            {
                var resultFile = Path.Combine(this.resultsPath, Guid.NewGuid().ToString("N") + ".json");
                var record = this.records.Dequeue();
                using (var fs = File.OpenWrite(resultFile))
                {
                    var sw = new StreamWriter(fs, encoding);
                    js.Serialize(sw, record);
                    sw.Flush();
                }
            }
        }

        public ResultRecordChunk Fetch(int maxRecords)
        {
            var encoding = new UTF8Encoding(false);
            var records = new ResultRecordChunk();

            var js = new Newtonsoft.Json.JsonSerializer();
            js.Converters.Add(new VersionConverter());

            foreach (var file in Directory.GetFiles(this.resultsPath))
            {
                if (records.Count >= maxRecords)
                {
                    break;
                }

                var guid = Guid.Parse(Path.GetFileNameWithoutExtension(file));
                try
                {
                    using (var fs = File.OpenRead(file))
                    {
                        var sr = new StreamReader(fs, encoding);
                        var jtr = new JsonTextReader(sr);
                        JToken token = JObject.Load(jtr);
                        var functiontype = (ModuleFunctionType)((int)token.SelectToken("FunctionType"));
                        var jr = token.CreateReader();
                        ModuleResultRecord record = null;

                        if (functiontype == ModuleFunctionType.Check)
                        {
                            record = js.Deserialize<CheckResultRecord>(jr);
                        }

                        if (functiontype == ModuleFunctionType.Info)
                        {
                            record = js.Deserialize<InfoResultRecord>(jr);
                        }

                        if (functiontype == ModuleFunctionType.Action)
                        {
                            record = js.Deserialize<ActionResultRecord>(jr);
                        }

                        records.Add(guid, record);
                    }
                }
                catch (Exception x)
                {
                    this.log.Warn(x, $"Could not open file \"{file}\": {x.Message}");
                }
            }

            return records;
        }

        public void Clear(Guid[] uploaded)
        {
            foreach (var guid in uploaded)
            {
                var file = Path.Combine(this.resultsPath, guid.ToString("N") + ".json");
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}

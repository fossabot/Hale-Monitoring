namespace Hale.Lib.JsonRpc
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class JsonRpcRequest
    {
        private const string Jsonrpc = "2.0";

        public string Method { get; set; }

        public object[] Params { get; set; }

        public string Id { get; set; }

        public static JsonRpcRequest FromJsonString(string jsonstring)
        {
            return JsonConvert.DeserializeObject<JsonRpcRequest>(jsonstring);
        }

        public static JsonRpcRequest FromJsonStream(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return FromJsonString(sr.ReadToEnd());
            }
        }

        public byte[] Serialize()
        {
            var js = new JsonSerializer();
            js.Converters.Add(new VersionConverter());
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                js.Serialize(sw, this);
            }

            return JsonRpcDefaults.Encoding.GetBytes(sb.ToString());
        }
    }
}

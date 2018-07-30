namespace Hale.Lib.JsonRpc
{
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class JsonRpcResponse
    {
        public const string JsonRpc = "2.0";

        public JsonRpcResponse()
        {
        }

        public JsonRpcResponse(JsonRpcRequest req)
        {
            this.Id = req.Id;
        }

        public object Result { get; set; }

        public JsonRpcError Error { get; set; }

        public string Id { get; set; }

        public static JsonRpcResponse FromRequest(JsonRpcRequest req)
        {
            return new JsonRpcResponse(req);
        }

        public static JsonRpcResponse FromJsonString(string jsonString)
        {
            var js = new JsonSerializer();
            js.Converters.Add(new VersionConverter());

            using (var sr = new StringReader(jsonString))
            {
                var jtr = new JsonTextReader(sr);
                return js.Deserialize<JsonRpcResponse>(jtr);
            }
        }

        public static JsonRpcResponse FromJsonStream(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return FromJsonString(streamReader.ReadToEnd());
            }
        }

        public byte[] Serialize()
        {
            string jsonString = JsonConvert.SerializeObject(this, JsonRpcDefaults.SerializerSettings);
            return JsonRpcDefaults.Encoding.GetBytes(jsonString);
        }
    }
}

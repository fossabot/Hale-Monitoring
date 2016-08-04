using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hale.Lib.JsonRpc
{
    public static class JsonRpcDefaults
    {
        public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static UTF8Encoding Encoding = new UTF8Encoding(false);
    }

    public class JsonRpcRequest
    {
        public readonly string Jsonrpc = "2.0";
        public string Method { get; set; }
        public object[] Params { get; set; }
        public string Id { get; set; }

        public byte[] Serialize()
        {
            string jsonstring = JsonConvert.SerializeObject(this, JsonRpcDefaults.SerializerSettings);
            return JsonRpcDefaults.Encoding.GetBytes(jsonstring);
        }

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
    }

    public class JsonRpcResponse
    {
        public readonly string Jsonrpc = "2.0";
        public object Result { get; set; }
        public JsonRpcError Error { get; set; }
        public string Id { get; set; }

        public static JsonRpcResponse FromRequest(JsonRpcRequest req)
        {
            return new JsonRpcResponse(req);
        }

        public JsonRpcResponse()
        {

        }

        public JsonRpcResponse(JsonRpcRequest req)
        {
            Id = req.Id;
        }

        public static JsonRpcResponse FromJsonString(string jsonString)
        {
            return JsonConvert.DeserializeObject<JsonRpcResponse>(jsonString);
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

    public class JsonRpcError
    {
        public JsonRpcErrorCode Code;
        public string Message;
        public Exception Data;
    }
}

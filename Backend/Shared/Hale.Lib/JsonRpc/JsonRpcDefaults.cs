namespace Hale.Lib.JsonRpc
{
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class JsonRpcDefaults
    {
        private static JsonSerializerSettings serializerSettings;
        private static UTF8Encoding encoding;

        public static JsonSerializerSettings SerializerSettings
            => serializerSettings != null
            ? serializerSettings
            : serializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public static UTF8Encoding Encoding
            => encoding != null
            ? encoding
            : encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    }
}

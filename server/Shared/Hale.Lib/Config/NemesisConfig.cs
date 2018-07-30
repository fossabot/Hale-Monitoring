namespace Hale.Lib
{
    using System;
    using System.IO;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class NemesisConfig
    {
        public string Hostname { get; set; }

        public int SendPort { get; set; }

        public int ReceivePort { get; set; }

        public bool UseEncryption { get; set; }

        public Guid Id { get; set; }

        private static INamingConvention NamingConvention
            => new CamelCaseNamingConvention();

        public static NemesisConfig LoadFromFile(string file)
        {
            using (var reader = File.OpenText(file))
            {
                Deserializer deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(NamingConvention)
                .Build();
                return deserializer.Deserialize<NemesisConfig>(reader);
            }
        }

        public void SaveToFile(string file)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(NamingConvention)
                .Build();
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(file)))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}

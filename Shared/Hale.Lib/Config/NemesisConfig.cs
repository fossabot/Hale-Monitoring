using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hale.Lib
{
    public class NemesisConfig
    {
        public string Hostname { get; set; }
        public ushort SendPort { get; set; }
        public ushort ReceivePort { get; set; }
        public bool UseEncryption { get; set; }
        public Guid Id { get; set; }
        public TimeSpan HeartBeatInterval { get; set; }

        public static NemesisConfig LoadFromFile(string file) 
        {
            using (var reader = File.OpenText(file))
            {
                Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
                return deserializer.Deserialize<NemesisConfig>(reader);
            }
        }
        public void SaveToFile(string file)
        {
            var serializer = new Serializer(namingConvention: new CamelCaseNamingConvention());
            using (StreamWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}

using Hale.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Hale.Core.Config
{
    public class CoreConfig
    {
        public AgentSection Agent { get; set; }
        public ApiSection Api { get; set; }
        public DatabaseSection Database { get; set; }

        private static INamingConvention NamingConvention 
            => new YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention();

        private static Encoding Encoding 
            => new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        public static CoreConfig Load(string filename)
        {
            var deserializer = new DeserializerBuilder()
                .WithTypeConverter(new IPAdressYamlTypeConverter())
                .WithNamingConvention(NamingConvention)
                .Build();

            using (var sr = new StreamReader(filename, Encoding))
            {
                return deserializer.Deserialize<CoreConfig>(sr);
            }
        }

        public void Save(string filename)
        {
            var serializer = new SerializerBuilder()
                .WithTypeConverter(new IPAdressYamlTypeConverter())
                .WithNamingConvention(NamingConvention)
                .Build();

            using (var sw = new StreamWriter(filename, append: false, encoding: Encoding))
            {
                serializer.Serialize(sw, this);
            }
        }

        public static CoreConfig Default => new CoreConfig
        {
            Agent = new AgentSection()
            {
                SendPort = 8988,
                ReceivePort = 8987,
                Hostname = "localhost",
                Ip = IPAddress.Loopback,
                UseEncryption = true
            },
            Api = new ApiSection()
            {
                Host = "+",
                Port = 8989,
                Scheme = "http",
                FrontendRoot = null,
            },
            Database = new DatabaseSection()
            {

            }
        };

        public class AgentSection
        {
            public int SendPort { get; set; }
            public int ReceivePort { get; set; }
            public string Hostname { get; set; }
            public IPAddress Ip { get; set; }
            public bool UseEncryption { get; set; }
        }

        public class ApiSection
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string Scheme { get; set; }
            public string FrontendRoot { get; set; }
        }

        public class DatabaseSection
        {

        }

    }
}

namespace Hale.Lib.Config
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public static class AgentConfigHandler
    {
        private static readonly Type[] LegacyConfigTypes =
        {
            typeof(AgentConfigBare) // V0
        };

        private static readonly Type CurrentConfigType = typeof(AgentConfig);

        private static Deserializer Deserializer => new DeserializerBuilder()
        .WithNamingConvention(new CamelCaseNamingConvention())
        .IgnoreUnmatchedProperties()
        .Build();

        public static AgentConfig LoadConfigFromFile(string file)
            => LoadConfig(() => File.OpenText(file));

        public static AgentConfig LoadConfigFromString(string input)
            => LoadConfig(() => new StringReader(input));

        private static AgentConfig LoadConfig(Func<TextReader> createReader)
        {
            AgentConfigBare bare;
            using (var reader = createReader())
            {
                bare = Deserializer.Deserialize<AgentConfigBare>(reader);
            }

            if (bare == null)
            {
                throw new FormatException("Invalid configuration format");
            }

            // NOTE: We need to create a new reader because there is no interface for resetting a TextReader -NM 2017-08-26
            using (var reader = createReader())
            {
                if (bare.Version == LegacyConfigTypes.Length)
                {
                    // Load the latest config version
                    return AgentConfig.Load(Deserializer.Deserialize<AgentConfigRaw>(reader));
                }
                else
                {
                    // Upgrade the configuration to the newest format
                    /*
                       Example:
                       var cv1 = AgentConfigV1.Load(Deserializer.Deserialize<AgentConfigRawV1>(reader));
                       return AgentConfig.Upgrade(cv1);
                     */
                    throw new NotImplementedException($"Unknown configuration file version {bare.Version}");
                }
            }
        }
    }
}

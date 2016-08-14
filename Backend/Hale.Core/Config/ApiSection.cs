using System.Configuration;

namespace Hale.Core.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiSection: ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("host")]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("port")]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("scheme")]
        public string Scheme
        {
            get
            {
                return (string)this["scheme"];
            }
            set
            {
                this["scheme"] = value;
            }
        }

        [ConfigurationProperty("frontendRoot")]
        public string FrontendRoot
        {
            get
            {
                return (string)this["frontendRoot"];
            }
            set
            {
                this["frontendRoot"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_config"></param>
        public static void ValidateSection(Configuration _config)
        {
            if (_config.Sections["api"] == null)
            {
                var section = new ApiSection
                {
                    Port = 8989,
                    Host = "+",
                    Scheme = "http",
                    FrontendRoot = @"frontend"
                };
                _config.Sections.Add("api", section);
            }
        }
    }
}

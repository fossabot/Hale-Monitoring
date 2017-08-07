using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Config
{
    public class CoreConfig
    {

        public static CoreConfig Load(string filename)
        {

        }

        public void Save(string filename)
        {

        }

        public class Agent
        {
            public int SendPort { get; set; } = 8988;

            public int ReceivePort { get; set; } = 8987;

            public string Hostname { get; set; }

            public IPAddress Ip { get; set; } = IPAddress.Loopback;

            public bool UseEncryption { get; set; }
        }

        public class Api
        {
            public string Host { get; set; } = "+";
            public int Port { get; set; } = 8989;
            public string Scheme { get; set; } = "http";
            public string FrontendRoot { get; set; } = null;
        }

        public class Database
        {

        }

        public class Environment
        {

            public string DataPath { get; set; }


            public string AgentDistPath { get; set; }


            public string ConfigFile { get; set; }


            public string NemesisConfigFile { get; set; }

     
            public string NemesisKeyFile { get; set; }
        }
    }
}

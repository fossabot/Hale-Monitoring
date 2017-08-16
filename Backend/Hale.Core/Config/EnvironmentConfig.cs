namespace Hale.Core.Config
{
    using System.IO;

    internal class EnvironmentConfig
    {
        private string dataPath;
        private string agentDistPath;
        private string configFile;
        private string nemesisConfigFile;
        private string nemesisKeyFile;
        private string modulePath;

        public string DataPath
        {
            get { return this.dataPath; }
            set { this.dataPath = AffirmPath(value); }
        }

        public string AgentDistPath
        {
            get { return this.agentDistPath; }
            set { this.agentDistPath = AffirmPath(value); }
        }

        public string ConfigFile
        {
            get { return this.configFile; }
            set { this.configFile = AffirmFilePath(value); }
        }

        public string NemesisConfigFile
        {
            get { return this.nemesisConfigFile; }
            set { this.nemesisConfigFile = AffirmFilePath(value); }
        }

        public string NemesisKeyFile
        {
            get { return this.nemesisKeyFile; }
            set { this.nemesisKeyFile = AffirmFilePath(value); }
        }

        public string ModulePath
        {
            get { return this.modulePath; }
            set { this.modulePath = AffirmPath(value); }
        }

        public static string AffirmPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string AffirmFilePath(string file)
        {
            AffirmPath(Path.GetDirectoryName(file));
            return file;
        }
    }
}

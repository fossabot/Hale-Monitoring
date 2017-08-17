namespace Hale.Agent.Config
{
    using System.IO;

    internal class EnvironmentConfig
    {
        private string dataPath;
        private string resultsPath;
        private string checksPath;
        private string configFile;
        private string nemesisConfigFile;
        private string nemesisKeyFile;

        public string DataPath
        {
            get { return this.dataPath; }
            set { this.dataPath = AffirmPath(value); }
        }

        public string ResultsPath
        {
            get { return this.resultsPath; }
            set { this.resultsPath = AffirmPath(value); }
        }

        public string ModulePath
        {
            get { return this.checksPath; }
            set { this.checksPath = AffirmPath(value); }
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
            Path.GetDirectoryName(file);
            return file;
        }
    }
}

namespace Hale.Core.Handlers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using Hale.Core.Config;
    using Hale.Core.Data.Contexts;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Lib.Utilities;
    using NLog;
    using Piksel.Nemesis.Security;
  
    internal partial class AgentDistHandler : IDisposable
    {
        private readonly HaleDBContext db = new HaleDBContext();
        private readonly ILogger log = LogManager.GetLogger("AgentDistHandler");

        // As these are low level variables, they shouldn't be changed during runtime.
        private readonly string distPath;
        private readonly RSAKey publicKey;
        private readonly string msifile = "Hale.Agent.msi";
        private readonly ushort coreSendPort;
        private readonly ushort coreReceivePort;
        private readonly string coreHostname;
        private readonly string platform = "platforms";
        private readonly string versionFile = "version.txt";

        private string target = "win32_i386"; // Todo: platform is @hardcoded for now @fixme -NM

        public AgentDistHandler()
        {
            // Todo: Move service provider calls to where the values are actually used and avoid critical
            var env = ServiceProvider.GetServiceCritical<EnvironmentConfig>();
            var agentHandler = ServiceProvider.GetServiceCritical<AgentHandler>();
            var agentConfig = ServiceProvider.GetServiceCritical<CoreConfig>().Agent;

            if (agentConfig.UseEncryption)
            {
                this.publicKey = agentHandler.PublicKey;
            }

            this.distPath = env.AgentDistPath;
            this.coreSendPort = (ushort)agentConfig.SendPort;
            this.coreReceivePort = (ushort)agentConfig.ReceivePort;
            this.coreHostname = agentConfig.Hostname;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        internal void CreatePackages(bool force = false)
        {
            var hosts = this.db.Nodes.ToList();

            // NM: @todo @performance @blocking make this threaded?
            foreach (Node host in hosts)
            {
                this.CreatePackage(host, force);
            }
        }

        private bool PackageIsLatest(string hostDistPath)
        {
            var hostVerstionPath = Path.Combine(hostDistPath, this.versionFile);

            var templateVersionPath = Path.Combine(this.distPath, this.platform, this.target, this.versionFile);
            if (!File.Exists(templateVersionPath))
            {
                this.GenerateTemplateVersionFile(templateVersionPath);
            }

            if (!File.Exists(hostVerstionPath))
            {
                return false;
            }

            return File.ReadAllText(hostVerstionPath) == File.ReadAllText(templateVersionPath);
        }


        private string CopyDistFiles(string hostDistPath, out string buildFile)
        {
            var outFile = Path.Combine(hostDistPath, this.msifile);
            buildFile = outFile + ".build";

            this.log.Debug("Copying dist files...");

            var tempPath = Path.Combine(this.distPath, this.platform, this.target);

            File.Copy(Path.Combine(tempPath, this.msifile), buildFile);
            File.Copy(Path.Combine(tempPath, this.versionFile), Path.Combine(hostDistPath, this.versionFile));

            return outFile;
        }

        private void CleanHostDistPath(string hostDistPath)
        {
            this.log.Debug("Cleaning output directory {0}...", hostDistPath);
            if (Directory.Exists(hostDistPath))
            {
                try
                {
                    Directory.Delete(hostDistPath, true);
                }
                catch (Exception x)
                {
                    throw new Exception($"Could not clean output directory. Got exception: {x.Message}. Skipping package...");
                }
            }

            var di = Directory.CreateDirectory(hostDistPath);
            var waitSeconds = 10;
            while (!di.Exists)
            {
                waitSeconds -= 1;

                if (waitSeconds == 0)
                {
                    throw new Exception("Timed out waiting for the OS to create our folder.");
                }

                Thread.Sleep(1000);
            }
        }

        private void RenameOutputFile(string outFile, string buildFile)
        {
            this.log.Debug("Renaming output file...");
            File.Move(buildFile, outFile);
        }

        private string GenerateNemesisConfig(Node host)
        {
            string nemesisConfig;
            string configPath = Path.Combine(this.distPath, "common", "nemesis.yaml");

            Directory.CreateDirectory(Path.Combine(this.distPath, "common"));

            if (!File.Exists(configPath))
            {
                using (var writer = File.OpenWrite(configPath))
                {
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Hale.Core.nemesis.yaml").CopyTo(writer);
                }
            }

            using (var sr = File.OpenText(configPath))
            {
                nemesisConfig = sr.ReadToEnd();
            }

            nemesisConfig = nemesisConfig.Replace("<HOSTNAME>", this.coreHostname);
            nemesisConfig = nemesisConfig.Replace("<SENDPORT>", this.coreReceivePort.ToString()); // Note: We swap the send/receive values here -NM
            nemesisConfig = nemesisConfig.Replace("<RECEIVEPORT>", this.coreSendPort.ToString());
            nemesisConfig = nemesisConfig.Replace("<GUID>", host.Guid.ToString());

            return nemesisConfig;
        }

    }
}

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
    using WindowsInstaller;

    internal class AgentDistHandler
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

        internal void CreatePackages(bool force = false)
        {
            var hosts = this.db.Nodes.ToList();

            // NM: @todo @performance @blocking make this threaded?
            foreach (Node host in hosts)
            {
                this.CreatePackage(host, force);
            }
        }

        // Thanks Microsoft!
        private MsiError UpdateProperty(int hMsi, string key, string value)
        {
            // var me = MsiInterop.MsiDatabaseOpenView(hMsi, "UPDATE Property SET Value = ? WHERE Property = ?", out hView);
            var me = MsiInterop.MsiDatabaseOpenView(hMsi, "INSERT INTO Property (Value, Property) VALUES (?, ?)", out int hView);
            if (me != MsiError.Success)
            {
                return me;
            }

            var hRec = MsiInterop.MsiCreateRecord(2);
            me = MsiInterop.MsiRecordSetString(hRec, 2, key);
            if (me != MsiError.Success)
            {
                return me;
            }

            me = MsiInterop.MsiRecordSetString(hRec, 1, value);
            if (me != MsiError.Success)
            {
                return me;
            }

            me = MsiInterop.MsiViewExecute(hView, hRec);
            if (me != MsiError.Success)
            {
                int errorRec = MsiInterop.MsiGetLastErrorRecord();
                uint errSize = 0;
                var sb = new StringBuilder();
                MsiInterop.MsiFormatRecord(0, errorRec, sb, ref errSize);
                sb = new StringBuilder((int)errSize);
                MsiInterop.MsiFormatRecord(0, errorRec, sb, ref errSize);
                this.log.Error("Database query error: {0}", sb.ToString());

                errSize = 0;
                sb = new StringBuilder();
                MsiInterop.MsiFormatRecord(0, hRec, sb, ref errSize);
                sb = new StringBuilder((int)errSize);
                MsiInterop.MsiFormatRecord(0, hRec, sb, ref errSize);
                this.log.Info("Record: {0}", sb.ToString());
            }

            return me;
        }

        private void CreatePackage(Node host, bool force = false)
        {
            var hostDistPath = Path.Combine(this.distPath, host.Guid.ToString());

            if (force || !this.PackageIsLatest(hostDistPath))
            {
                this.log.Info("Creating dist package for host {0}...", host.Guid.ToString());

                try
                {
                    this.CleanHostDistPath(hostDistPath);

                    string buildFile;
                    string outFile = this.CopyDistFiles(hostDistPath, out buildFile);

                    var hMsi = this.OpenMsiDatabase(buildFile);
                    this.SetAgentGuid(host, hMsi);
                    this.SetAgentKeys(host, hMsi);
                    this.SetCoreKey(hMsi);
                    this.SetCoreHostname(hMsi);
                    this.SetCorePorts(hMsi);

                    string nemesisConfig = this.GenerateNemesisConfig(host);
                    this.SetNemesisConfig(hMsi, nemesisConfig);
                    this.CommitToMsiDatabase(hMsi);

                    this.CloseMsiDatabase(hMsi);
                    MsiInterop.MsiCloseAllHandles();
                    this.RenameOutputFile(outFile, buildFile);
                }
                catch (Exception e)
                {
                    this.log.Debug(e.Message);
                }
            }
            else
            {
                this.log.Debug($"Skipping dist package creation for host {host.Guid.ToString()}...");
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

        private void GenerateTemplateVersionFile(string templateVersionPath)
        {
            var versionKey = "ProductVersion";
            uint size = 255;

            var templateMsi = Path.Combine(this.distPath, this.platform, this.target, this.msifile);
            var hMsi = this.OpenMsiPackage(templateMsi);

            StringBuilder sb = new StringBuilder((int)size);

            var me = MsiInterop.MsiGetProperty(hMsi, versionKey, sb, ref size);

            if (me == MsiError.MoreData)
            {
                size++; // Note: Adding space for terminating null character -NM
                sb = new StringBuilder((int)size);
                me = MsiInterop.MsiGetProperty(hMsi, versionKey, sb, ref size);
            }

            if (me == MsiError.Success)
            {
                File.WriteAllText(templateVersionPath, sb.ToString());
            }
            else
            {
                throw new Exception($"MSI Exception {me.ToString()}");
            }
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

        private void CloseMsiDatabase(int hMsi)
        {
            this.log.Debug("Closing MSI database...");
            MsiError me = MsiInterop.MsiCloseHandle(hMsi);
            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private void CommitToMsiDatabase(int hMsi)
        {
            this.log.Debug("Commiting changes to MSI database...");
            MsiError me = MsiInterop.MsiDatabaseCommit(hMsi);
            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private void SetNemesisConfig(int hMsi, string nemesisConfig)
        {
            this.log.Debug("Setting nemesis config...");
            MsiError me = this.UpdateProperty(hMsi, "HALE_AGENT_NEMESIS_CONFIG", nemesisConfig);

            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
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

        private void SetCorePorts(int hMsi)
        {
            this.log.Debug("Setting core port...");
            MsiError me = this.UpdateProperty(hMsi, "HALE_CORE_SEND_PORT", this.coreSendPort.ToString());
            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }

            me = this.UpdateProperty(hMsi, "HALE_CORE_RECEIVE_PORT", this.coreReceivePort.ToString());
            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private void SetCoreHostname(int hMsi)
        {
            this.log.Debug("Setting core hostname...");
            MsiError me = this.UpdateProperty(hMsi, "HALE_CORE_HOSTNAME", this.coreHostname);

            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private void SetCoreKey(int hMsi)
        {
            this.log.Debug("Setting core key...");
            var xmlKeyCore = RSA.Default.ExportToXml(this.publicKey.Key, false);
            MsiError me = this.UpdateProperty(hMsi, "HALE_CORE_KEY", xmlKeyCore);

            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private void SetAgentKeys(Node host, int hMsi)
        {
            this.log.Debug("Setting agent keys...");
            var xmlKeyAgent = RSA.Default.ExportToXml(host.RsaKey);
            MsiError me = this.UpdateProperty(hMsi, "HALE_AGENT_KEYS", xmlKeyAgent);

            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private void SetAgentGuid(Node host, int hMsi)
        {
            this.log.Debug("Setting agent GUID...");
            MsiError me = this.UpdateProperty(hMsi, "HALE_AGENT_GUID", host.Guid.ToString());

            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }
        }

        private int OpenMsiDatabase(string msiFile, MsiDbPersistMode mode = MsiDbPersistMode.Transact)
        {
            int hMsi;

            this.log.Debug("Opening MSI database...");
            MsiError me = MsiInterop.MsiOpenDatabase(msiFile, mode, out hMsi);
            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }

            return hMsi;
        }

        private int OpenMsiPackage(string msiFile)
        {
            int hMsi;

            this.log.Debug("Opening MSI database...");
            MsiError me = MsiInterop.MsiOpenPackage(msiFile, out hMsi);
            if (me != MsiError.Success)
            {
                throw new Exception(message: $"Error: {me.ToString()}");
            }

            return hMsi;
        }
    }
}

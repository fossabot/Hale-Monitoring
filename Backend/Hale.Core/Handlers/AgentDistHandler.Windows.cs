#if NET471
using Hale.Core.Data.Entities.Nodes;
using Piksel.Nemesis.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInstaller;

namespace Hale.Core.Handlers
{
    internal partial class AgentDistHandler
    {
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
#endif
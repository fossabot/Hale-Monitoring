namespace Hale.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Hale.Core.Data.Contexts;
    using Hale.Core.Data.Entities.Modules;
    using Hale.Lib.ModuleLoader;
    using Hale.Lib.Modules;
    using NLog;
    using EModule = Hale.Core.Data.Entities.Modules.Module;

    internal enum ModuleArchiveCompression
    {
        Lzma, Deflate, Bzip2
    }

    internal class ModuleHandler : IDisposable
    {
        private ILogger log = LogManager.GetLogger("ModuleHandler");
        private HaleDBContext db = new HaleDBContext();

        public void ScanForModules(string scanPath)
        {
            this.log.Info($"Scanning path \"{scanPath}\"...");
            foreach (var file in this.ScanDir(scanPath))
            {
                var shortPath = file.Replace(scanPath, string.Empty);
                this.log.Debug($" - Found module at \"{shortPath}\".");
                var mri = ModuleLoader.GetModuleInfo(Path.GetFileName(file), Path.GetDirectoryName(file));
                if (mri != null)
                {
                    this.AddModule(mri);
                }
            }
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        private void AddModule(ModuleRuntimeInfo mi)
        {
            this.log.Info($"Adding module <{mi.Module}>, {mi.Functions[ModuleFunctionType.Action].Count} action-, " +
                $"{mi.Functions[ModuleFunctionType.Info].Count} info- and {mi.Functions[ModuleFunctionType.Check].Count} check-functions.");

            EModule me = this.db.Modules.FirstOrDefault(m =>
                    m.Major == mi.Module.Version.Major &&
                    m.Minor == mi.Module.Version.Minor &&
                    m.Revision == mi.Module.Version.Patch &&
                    m.Identifier == mi.Module.Identifier);

            if (me == null)
            {
                try
                {
                    me = new EModule()
                    {
                        Major = mi.Module.Version.Major,
                        Minor = mi.Module.Version.Minor,
                        Revision = mi.Module.Version.Patch,
                        Identifier = mi.Module.Identifier
                    };
                    this.db.Modules.Add(me);
                }
                catch (Exception x)
                {
                    this.log.Warn($"Could not add module <{mi}> to database: {x.Message}");
                    return;
                }
            }

            foreach (var funcType in mi.Functions)
            {
                foreach (var fn in funcType.Value)
                {
                    try
                    {
                        var mf = new Function()
                        {
                            Type = funcType.Key,
                            Name = fn.Value.Name,
                            Identifier = fn.Key,
                            Description = fn.Value.Description,
                            Module = me
                        };
                        this.db.Functions.Add(mf);
                    }
                    catch (Exception x)
                    {
                        this.log.Warn($"Could not add module function <{mi}>::{fn} to database: {x.Message}");
                        return;
                    }
                }
            }

            this.db.SaveChanges();
        }

        private string[] ScanDir(string scanPath)
        {
            List<string> files = new List<string>();
            foreach (var dir in Directory.GetDirectories(scanPath))
            {
                files.AddRange(Directory.GetFiles(dir, "HaleModule*CPU.dll"));
                files.AddRange(this.ScanDir(dir));
            }

            return files.ToArray();
        }
    }
}

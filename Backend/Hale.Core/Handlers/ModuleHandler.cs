using Hale.Core.Data.Contexts;
using Hale.Lib.ModuleLoader;
using Hale.Lib.Modules;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization.NamingConventions;
using Module = Hale.Core.Data.Entities.Module;

namespace Hale.Core.Handlers
{
    class ModuleHandler
    {
        ILogger _log = LogManager.GetLogger("ModuleHandler");
        HaleDBContext _db = new HaleDBContext();

        public void ScanForModules(string scanPath)
        {
            _log.Info($"Scanning path \"{scanPath}\"...");
            foreach(var file in scanDir(scanPath))
            {
                var shortPath = file.Replace(scanPath, "");
                _log.Debug($" - Found module at \"{shortPath}\".");
                var mri = ModuleLoader.GetModuleInfo(Path.GetFileName(file), Path.GetDirectoryName(file));
                if(mri != null)
                    AddModule(mri);
            }
        }

        private void AddModule(ModuleRuntimeInfo mi)
        {
            _log.Info($"Adding module <{mi.Module}>, {mi.Functions[ModuleFunctionType.Action].Count} action-, "+
                $"{mi.Functions[ModuleFunctionType.Info].Count} info- and {mi.Functions[ModuleFunctionType.Check].Count} check-functions.");

            Module me = _db.Modules.FirstOrDefault(m =>
                    m.Major == mi.Module.Version.Major &&
                    m.Minor == mi.Module.Version.Minor &&
                    m.Revision == mi.Module.Version.Revision &&
                    m.Identifier == mi.Module.Identifier);
                

            if(me == null)
            {
                try
                {
                    me = _db.Modules.Add(new Module()
                {
                    Major = mi.Module.Version.Major,
                    Minor = mi.Module.Version.Minor,
                    Revision = mi.Module.Version.Revision,
                    Identifier = mi.Module.Identifier
                });
                }
                catch (Exception x)
                {
                    _log.Warn($"Could not add module <{mi}> to database: {x.Message}");
                    return;
                }
            }


            foreach (var funcType in mi.Functions) {
                foreach (var fn in funcType.Value) {
                    try
                    {
                        var mf = new Data.Entities.Function()
                        {
                            Type = funcType.Key,
                            Name = fn.Value.Name,
                            ModuleId = me.Id
                        };
                        _db.Functions.Add(mf);
                    }
                    catch (Exception x)
                    {
                        _log.Warn($"Could not add module function <{mi}>::{fn} to database: {x.Message}");
                        return;
                    }
                }
            }


            _db.SaveChanges();

        }

        internal void ScanForModules(object modulePath)
        {
            throw new NotImplementedException();
        }

        private string[] scanDir(string scanPath)
        {
            List<string> files = new List<string>();
            foreach (var dir in Directory.GetDirectories(scanPath))
            {
                files.AddRange(Directory.GetFiles(dir, "HaleModule*CPU.dll"));
                files.AddRange(scanDir(dir));
            }
            return files.ToArray();
        }

        public ModuleInfo GetModuleInfo(string modulePath)
        {
            
            try
            {

                var mi = new ModuleInfo();
                mi.ModulePath = Path.GetDirectoryName(modulePath);

                mi.GetRuntimeInfo(modulePath);
                var mri = ModuleLoader.GetModuleInfo(Path.GetFileName(modulePath), Path.GetDirectoryName(modulePath));
                return mi;
            }
            catch (Exception x)
            {
                _log.Warn($"Error probing module: {x.Message}");
            }
            return null;
        }
    }

    class ModuleInfo
    {
        public Version Version
        {
            get { return Module.Version; }
            set { Module.Version = value; }
        }

        public string Identifier
        {
            get { return Module.Identifier; }
            set { Module.Identifier = value; }
        }

        public override string ToString()
        {
            return Module.ToString();
        }

        public string ModulePath;

        public List<string> CheckFunctions = new List<string>();
        public List<string> InfoFunctions = new List<string>();
        public List<string> ActionFunctions = new List<string>();

        public bool NeedsRuntimeInfo = true;
        public bool Valid = false;

        public ModuleManifest Manifest;

        /// <summary>
        ///  The version specified inside the manifest, or if missing, the module dll version
        /// </summary>
        public VersionedIdentifier Module;

        /// <summary>
        ///  The version of the module dll
        /// </summary>
        public VersionedIdentifier Runtime;

        public static ModuleInfo FromManifest(ModuleManifest manifest)
        {
            var mi = new ModuleInfo();
            mi.Manifest = manifest;
            if (!string.IsNullOrEmpty(manifest.Information.Identifier) && manifest.Information.Version != null)
                mi.Module = new VersionedIdentifier(manifest.Information.Identifier,
                    manifest.Information.Version);
            return mi;
        }

        public void GetRuntimeInfo()
        {
            GetRuntimeInfo(Path.Combine(ModulePath, Manifest.Module.Filename));
        }

        public void GetRuntimeInfo(string runtimePath)
        {
            if (File.Exists(runtimePath))
            {
                var mri = ModuleLoader.GetModuleInfo(Path.GetFileName(runtimePath), Path.GetDirectoryName(runtimePath));
                Runtime = mri.Module;
                if (Module == null)
                    Module = mri.Module;
                CheckFunctions = mri.Functions[ModuleFunctionType.Check].Keys.ToList();
                InfoFunctions = mri.Functions[ModuleFunctionType.Info].Keys.ToList();
                ActionFunctions = mri.Functions[ModuleFunctionType.Action].Keys.ToList();
                NeedsRuntimeInfo = false;
                Valid = true;
            }
            else
            {
                Valid = false;
                throw new FileNotFoundException($"Module runtime file \"{runtimePath}\" not found.");
            }
        }

        public Module GetModuleEntity()
        {
            return new Module() { Version = Version, Identifier = Identifier };
        }
    }

    enum ModuleArchiveCompression
    {
        Lzma, Deflate, Bzip2
    }
}

namespace Hale.Lib.ModuleLoader
{
    using System;
    using System.IO;
    using System.Runtime.Remoting;
    using System.Security;
    using System.Security.Permissions;
    using Hale.Lib.Modules;

    public class ModuleLoader : MarshalByRefObject
    {
        public static TResult ExecuteFunction<TResult>(string dll, string modulePath, string name, ModuleSettingsBase settings)
            where TResult : ModuleResultSet, new()
        {
            ModuleDomain moduleDomain = GetDomain(dll, modulePath);
            return moduleDomain.ExecuteFunction<TResult>(Path.GetFullPath(Path.Combine(modulePath, dll)), name, settings);
        }
        
        public static ModuleRuntimeInfo GetModuleInfo(string dll, string modulePath)
        {
            ModuleDomain moduleDomain = GetDomain(dll, modulePath);
            return moduleDomain.GetModuleInfo(Path.GetFullPath(Path.Combine(modulePath, dll)));
        }

        private static ModuleDomain GetDomain(string dll, string checkPath)
        {
            AppDomainSetup adSetup = new AppDomainSetup();

            adSetup.ApplicationBase = Path.GetFullPath(checkPath);
            PermissionSet permSet = new PermissionSet(PermissionState.Unrestricted);
            AppDomain newDomain = AppDomain.CreateDomain("Module::" + Path.GetFileNameWithoutExtension(dll), null, adSetup, permSet);

            ObjectHandle handle = Activator.CreateInstanceFrom(
                newDomain,
                typeof(ModuleDomain).Assembly.ManifestModule.FullyQualifiedName,
                typeof(ModuleDomain).FullName);

            return (ModuleDomain)handle.Unwrap();
        }
    }
}

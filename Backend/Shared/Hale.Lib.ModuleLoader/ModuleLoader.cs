namespace Hale.Lib.ModuleLoader
{
    using System;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;
    using Hale.Lib.Modules;

    public partial class ModuleLoader : MarshalByRefObject
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
    }
}

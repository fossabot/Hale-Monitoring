#if NET471
namespace Hale.Lib.ModuleLoader
{
    using System;
    using System.IO;
    using System.Runtime.Remoting;
    using System.Security;
    using System.Security.Permissions;

    public partial class ModuleLoader
    {
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
#endif
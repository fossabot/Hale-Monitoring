namespace Hale.Modules
{
    using System.ServiceProcess;
    using Hale.Lib.Modules.Attributes;

    [HaleModule("com.itshale.core.service", 0, 1, 1)]
    [HaleModuleName("Service Module")]
    [HaleModuleAuthor("Hale Project")]

    public partial class ServiceModule
    {
        private static readonly ServiceControllerStatus[] CriticalStatuses =
        {
            ServiceControllerStatus.Stopped,
            ServiceControllerStatus.StopPending
        };
    }
}

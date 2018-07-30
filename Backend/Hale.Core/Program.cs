namespace Hale.Core
{
    using System;
    using System.ServiceProcess;

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
#if !DEBUG
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new HaleCoreService()
            };
            ServiceBase.Run(ServicesToRun);
#else
            Console.Title = "Hale Core";
            HaleCoreService svc = new HaleCoreService();
            svc.DebugStart();
#endif
        }
    }
}

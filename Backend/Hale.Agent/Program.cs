namespace Hale.Agent
{
    using System;
    using System.ServiceProcess;
    using NLog;

    public class Program : MarshalByRefObject
    {
        private static readonly ILogger Log = LogManager.GetLogger("Main");

        private static void Main()
        {
#if DEBUG
            Console.Title = "Hale Agent";
            Log.Info("Starting Agent in Debug mode.");
            HaleAgentService svc = new HaleAgentService();
            svc.StartDebug();
#else
            Log.Info("Starting Hale Agent in Service mode.");
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new HaleAgentService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}

using System;
using Hale.Agent;
using NLog;
using System.ServiceProcess;

namespace Hale.Agent
{
    class Program: MarshalByRefObject
    {
        static ILogger _log = LogManager.GetLogger("Main");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            Console.Title = "Hale Agent";
            _log.Info("Starting Agent in Debug mode.");
            HaleAgentService svc = new HaleAgentService();
            svc.StartDebug();
#else
            _log.Info("Starting Hale Agent in Service mode.");
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

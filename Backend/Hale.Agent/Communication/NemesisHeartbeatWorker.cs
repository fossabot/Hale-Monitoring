namespace Hale.Agent.Communication
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using Hale.Lib;
    using Hale.Lib.JsonRpc;
    using NLog;
    using Piksel.Nemesis;

    internal class NemesisHeartbeatWorker
    {
        private NemesisConfig config;
        private BackgroundWorker worker;
        private NemesisNode node;

        public NemesisHeartbeatWorker(NemesisConfig config, NemesisNode node)
        {
            this.config = config;
            this.node = node;
        }

        public void Start()
        {
            this.worker = new BackgroundWorker();
            this.worker.DoWork += new DoWorkEventHandler(this.DoWork);
            this.worker.RunWorkerAsync(this.config);
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var config = (NemesisConfig)e.Argument;
            var worker = (BackgroundWorker)sender;
            var lastRun = DateTime.MinValue;

            ILogger log = LogManager.GetLogger("NemesisHeartbeat");

            while (true)
            {
                if (!worker.CancellationPending)
                {
                    if (DateTime.Now - lastRun > this.config.HeartBeatInterval)
                    {
                        log.Debug("Sending heartbeat to Core...");
                        lastRun = DateTime.Now;

                        var req = new JsonRpcRequest()
                        {
                            Method = "heartbeat"
                        };

                        try
                        {
                            var res = this.node.SendCommand(JsonRpcDefaults.Encoding.GetString(req.Serialize()));
                            log.Debug("Got response: {0}", res.Result);
                        }
                        catch (Exception x)
                        {
                            log.Error("Error when sending heartbeat: {0}", x.Message);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void Stop()
        {
            this.worker.CancelAsync();
        }
    }
}

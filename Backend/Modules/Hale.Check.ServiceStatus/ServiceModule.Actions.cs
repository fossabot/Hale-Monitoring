namespace Hale.Modules
{
    using System;
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Results;

    public partial class ServiceModule
    {
        [ActionFunction(Identifier = "start")]
        public ActionResultSet StartServiceAction(ActionSettings settings)
        {
            var afr = new ActionResultSet();

            var globalTimeout = settings.ContainsKey("timeout") ?
                TimeSpan.Parse(settings["timeout"]) :
                TimeSpan.FromSeconds(6);

            foreach (var kvpTarget in settings.TargetSettings)
            {
                var result = this.ChangeServiceState(settings, kvpTarget.Value, kvpTarget.Key, ServiceControllerStatus.Running, globalTimeout);
                afr.ActionResults.Add(kvpTarget.Key, result);
            }

            afr.RanSuccessfully = true;

            return afr;
        }

        [ActionFunction(Identifier = "stop")]
        public ActionResultSet StopServiceAction(ActionSettings settings)
        {
            var afr = new ActionResultSet();

            var globalTimeout = settings.ContainsKey("timeout") ?
                TimeSpan.Parse(settings["timeout"]) :
                TimeSpan.FromSeconds(6);

            foreach (var kvpTarget in settings.TargetSettings)
            {
                var result = this.ChangeServiceState(settings, kvpTarget.Value, kvpTarget.Key, ServiceControllerStatus.Stopped, globalTimeout);
                afr.ActionResults.Add(kvpTarget.Key, result);
            }

            afr.RanSuccessfully = true;

            return afr;
        }

        [ActionFunction(Identifier = "restart")]
        public ActionResultSet RestartServiceAction(ActionSettings settings)
        {
            var afr = new ActionResultSet();

            var globalTimeout = settings.ContainsKey("timeout") ?
                TimeSpan.Parse(settings["timeout"]) :
                TimeSpan.FromSeconds(6);

            foreach (var kvpTarget in settings.TargetSettings)
            {
                var start = DateTime.UtcNow;
                var resultStop = this.ChangeServiceState(settings, kvpTarget.Value, kvpTarget.Key, ServiceControllerStatus.Stopped, globalTimeout);
                if (resultStop.RanSuccessfully)
                {
                    var resultStart = this.ChangeServiceState(settings, kvpTarget.Value, kvpTarget.Key, ServiceControllerStatus.Running, globalTimeout);
                    if (resultStart.RanSuccessfully)
                    {
                        afr.ActionResults.Add(kvpTarget.Key, new ActionResult()
                        {
                            RanSuccessfully = true,
                            Message = $"Service {kvpTarget.Key} successfully restarted."
                        });
                    }
                    else
                    {
                        afr.ActionResults.Add(kvpTarget.Key, new ActionResult()
                        {
                            RanSuccessfully = false,
                            Message = resultStart.Message,
                            ExecutionException = resultStart.ExecutionException
                        });
                    }
                }
                else
                {
                    afr.ActionResults.Add(kvpTarget.Key, new ActionResult()
                    {
                        RanSuccessfully = false,
                        Message = resultStop.Message,
                        ExecutionException = resultStop.ExecutionException
                    });
                }
            }

            afr.RanSuccessfully = true;

            return afr;
        }

        private ActionResult ChangeServiceState(ActionSettings settings, Dictionary<string, string> targetSettings, string target, ServiceControllerStatus newStatus, TimeSpan globalTimeout)
        {
            var result = new ActionResult();
            try
            {
                var timeout = settings.ContainsKey("timeout") ?
                        TimeSpan.Parse(settings["timeout"]) :
                        globalTimeout;
                var sc = targetSettings.ContainsKey("machine") ?
                    new ServiceController(target, targetSettings["machine"]) :
                    new ServiceController(target);
                var start = DateTime.UtcNow;
                if (newStatus == ServiceControllerStatus.Running)
                {
                    sc.Start();
                }

                if (newStatus == ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                }

                sc.WaitForStatus(newStatus, timeout);
                var elapsed = DateTime.UtcNow - start;
                if (sc.Status == newStatus)
                {
                    result.RanSuccessfully = true;
                    if (newStatus == ServiceControllerStatus.Running)
                    {
                        result.Message = $"Service {target} was started successfully in {elapsed.TotalSeconds:f1} seconds.";
                    }

                    if (newStatus == ServiceControllerStatus.Stopped)
                    {
                        result.Message = $"Service {target} was stopped successfully in {elapsed.TotalSeconds:f1} seconds.";
                    }
                }
                else
                {
                    result.RanSuccessfully = false;
                    if (newStatus == ServiceControllerStatus.Running)
                    {
                        result.Message = $"Service {target} failed to start, current status is {sc.Status.ToString()}.";
                    }

                    if (newStatus == ServiceControllerStatus.Stopped)
                    {
                        result.Message = $"Service {target} failed to stop, current status is {sc.Status.ToString()}.";
                    }

                    result.ExecutionException = new ServiceCommandException("Timed out while waiting for service status.");
                }
            }
            catch (Exception x)
            {
                result.ExecutionException = x;
                result.Message = x.Message;
                result.RanSuccessfully = false;
            }

            return result;
        }
    }
}

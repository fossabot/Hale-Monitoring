namespace Hale.Alerts
{
    using System;
    using System.Collections.Generic;
    using Hale.Alert.Pushbullet;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Alerts;
    using Hale.Lib.Modules.Results;
    using Module = Hale.Lib.Modules.Module;

    public class HaleAlertPushbullet : Module, IAlertProvider
    {
        private PushbulletApi api;

        public override string Name => "Pushbullet";

        public override string Author => "Hale Project";

        public override string Identifier => "com.itshale.core.pushbullet";

        public override Version Version => new Version(0, 1, 1);

        public override string Platform => "Windows";

        public override decimal TargetApi { get; } = 1.3M;

        public bool Ready { get; private set; }

        public string InitResponse { get; private set; }

        public AlertResultSet DefaultAlert(AlertSettings settings)
        {
            var afr = new AlertResultSet();
            foreach (var target in settings.Targets)
            {
                var ar = new AlertResult();
                try
                {
                    var targetSettings = settings.TargetSettings[target];
                    var recipient = new HaleAlertPushbulletRecipient()
                    {
                        AccessToken = targetSettings["accessToken"],
                        Target = targetSettings["target"],
                        TargetType = (PushbulletPushTarget)Enum.Parse(typeof(PushbulletPushTarget), targetSettings["targetType"]),
                        Name = targetSettings["name"],
                        Id = Guid.NewGuid()
                    };
                    var response = this.api.Push("Hale Alert", string.Format("{0}\n{1}", settings.SourceString, settings.Message), recipient);

                    ar.Message = response.Type;
                    ar.RanSuccessfully = true;
                }
                catch (Exception x)
                {
                    ar.RanSuccessfully = false;
                    ar.ExecutionException = x;
                }

                afr.AlertResults.Add(target, ar);
            }

            afr.RanSuccessfully = true;
            return afr;
        }

        public void InitializeAlertProvider(AlertSettings settings)
        {
            this.AddAlertFunction(this.DefaultAlert);
            this.api = new PushbulletApi();
        }
    }
}

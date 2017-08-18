namespace Hale.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Alerts;
    using Hale.Lib.Modules.Results;

    public class SimpleHaleAlert : Hale.Lib.Modules.Module, IAlertProvider
    {
        public override string Name { get; } = "Simple Alert";

        public override string Author { get; } = "Hale Project";

        public override string Identifier { get; } = "com.itshale.core.simplealert";

        public override Version Version { get; } = new Version(0, 1, 1);

        public override string Platform { get; } = "Windows";

        public new decimal TargetApi { get; } = 1.3M;

        public AlertResultSet DefaultAlert(AlertSettings settings)
        {
            var afr = new AlertResultSet();
            foreach (var target in settings.Targets)
            {
                var result = new AlertResult();
                var mbresult = MessageBox.Show(
                    settings.Message,
                    settings.SourceString,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                result.RanSuccessfully = true;
                result.Message = $"{target} -> {mbresult.ToString()}";
                result.Target = target;
                afr.AlertResults.Add(target, result);
            }

            afr.RanSuccessfully = true;
            return afr;
        }

        public void InitializeAlertProvider(AlertSettings settings)
        {
            this.AddAlertFunction(this.DefaultAlert);
        }
    }
}

namespace Hale.Lib.Modules.Alerts
{
    using Hale.Lib.Modules.Results;

    public delegate AlertFunctionResult AlertFunction(AlertSettings settings);

    public interface IAlertProvider : IModuleProviderBase
    {
        void InitializeAlertProvider(AlertSettings settings);
    }

    public static class AlertProviderExtensions
    {
        public static AlertFunctionResult ExecuteAlertFunction(this IAlertProvider alertProvider, string action, AlertSettings settings)
        {
            return alertProvider.ExecuteFunction(action, settings, "alert") as AlertFunctionResult;
        }

        public static void AddAlertFunction(this IAlertProvider alertProvider, string alert, AlertFunction func)
        {
            alertProvider.Functions.Add("alert_" + alert, (settings) =>
            {
                return func(settings as AlertSettings);
            });
        }

        public static void AddAlertFunction(this IAlertProvider alertProvider, AlertFunction func)
        {
            alertProvider.AddAlertFunction("default", func);
        }
    }
}

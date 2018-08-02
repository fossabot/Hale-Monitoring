namespace Hale.Lib.Modules.Info
{
    using Hale.Lib.Modules.Results;

    public static class InfoProviderExtensions
    {
        public static InfoResultSet ExecuteInfoFunction(this IInfoProvider infoProvider, string name, InfoSettings settings)
        {
            return infoProvider.ExecuteFunction(name, settings, "info") as InfoResultSet;
        }

        public static void AddInfoFunction(this IInfoProvider infoProvider, string name, InfoFunction func)
        {
            infoProvider.Functions.Add("info_" + name, (settings) =>
            {
                return func(settings as InfoSettings);
            });
        }

        public static void AddInfoFunction(this IInfoProvider infoProvider, InfoFunction func)
        {
            infoProvider.AddInfoFunction("default", func);
        }

        public static void AddSingleResultInfoFunction(this IInfoProvider infoProvider, string name, SingleResultInfoFunction func)
        {
            infoProvider.Functions.Add("info_" + name, (settings) =>
            {
                var ir = func(settings as InfoSettings);
                var ifr = new InfoResultSet();
                ifr.InfoResults.Add("default", ir);
                ifr.RanSuccessfully = ir.RanSuccessfully;
                ifr.FunctionException = ir.ExecutionException;
                ifr.Message = ir.Message;
                return ifr;
            });
        }

        public static void AddSingleResultInfoFunction(this IInfoProvider infoProvider, SingleResultInfoFunction func)
        {
            infoProvider.AddSingleResultInfoFunction("default", func);
        }
    }
}

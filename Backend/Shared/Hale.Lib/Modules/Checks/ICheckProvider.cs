namespace Hale.Lib.Modules.Checks
{
    using Hale.Lib.Modules.Results;

    public delegate CheckResultSet CheckFunction(CheckSettings settings);

    /// <summary>
    ///  A simplified delegate without the function result wrapper
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public delegate CheckResult SingleResultCheckFunction(CheckSettings settings);

    /// <summary>
    /// Represents a Hale module with exposed check functions
    /// </summary>
    public interface ICheckProvider : IModuleProviderBase
    {
        /// <summary>
        /// Sets up global check settings and exposes check functions
        /// </summary>
        /// <param name="settings"></param>
        void InitializeCheckProvider(CheckSettings settings);
    }

    /// <summary>
    /// Extensions to the interface that makes it easier to create hale modules.
    /// </summary>
    public static class CheckProviderExtensions
    {
        /// <summary>
        /// Executes the specific check function
        /// </summary>
        /// <param name="checkProvider"></param>
        /// <param name="name">The exposed name of the function</param>
        /// <param name="settings">The target settings object to pass to the function</param>
        /// <returns></returns>
        public static CheckResultSet ExecuteCheckFunction(this ICheckProvider checkProvider, string name, CheckSettings settings)
        {
            return checkProvider.ExecuteFunction(name, settings, "check") as CheckResultSet;
        }

        /// <summary>
        /// Adds a named check function
        /// </summary>
        /// <param name="checkProvider"></param>
        /// <param name="name">Exposed name of the function</param>
        /// <param name="func">Method that matches delegate CheckFunction</param>
        public static void AddCheckFunction(this ICheckProvider checkProvider, string name, CheckFunction func)
        {
            checkProvider.Functions.Add("check_" + name, (settings) =>
            {
                return func(settings as CheckSettings);
            });
        }

        /// <summary>
        /// Adds check function as default function
        /// </summary>
        /// <param name="checkProvider"></param>
        /// <param name="func">Method that matches delegate CheckFunction</param>
        public static void AddCheckFunction(this ICheckProvider checkProvider, CheckFunction func)
        {
            checkProvider.AddCheckFunction("default", func);
        }

        /// <summary>
        ///  Adds a named check function with a single result
        /// </summary>
        /// <param name="checkProvider"></param>
        /// <param name="name">Exposed name of the function</param>
        /// <param name="func">Method that matches delegate SingleResultCheckFunction</param>
        public static void AddSingleResultCheckFunction(this ICheckProvider checkProvider, string name, SingleResultCheckFunction func)
        {
            checkProvider.Functions.Add("check_" + name, (settings) =>
            {
                var cr = func(settings as CheckSettings);
                var cfr = new CheckResultSet();
                cfr.CheckResults.Add("default", cr);
                cfr.RanSuccessfully = cr.RanSuccessfully;
                cfr.FunctionException = cr.ExecutionException;
                cfr.Message = cr.Message;
                return cfr;
            });
        }

        /// <summary>
        /// Adds check function as default function with single result
        /// </summary>
        /// <param name="checkProvider"></param>
        /// <param name="func">Method that matches delegate SingleResultCheckFunction</param>
        public static void AddSingleResultCheckFunction(this ICheckProvider checkProvider, SingleResultCheckFunction func)
        {
            checkProvider.AddSingleResultCheckFunction("default", func);
        }
    }
}

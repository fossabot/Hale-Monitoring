using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hale.Lib.Modules;
using Hale.Lib.Modules.Actions;

namespace Hale.Lib.Modules.Actions
{
    public interface IActionProvider: IModuleProviderBase
    {
        void InitializeActionProvider(ActionSettings settings);
    }

    public delegate ActionFunctionResult ActionFunction(ActionSettings settings);
    public delegate ActionResult SingleResultActionFunction(ActionSettings settings);

    public static class ActionProviderExtensions
    {
        public static ActionFunctionResult ExecuteActionFunction(this IActionProvider actionProvider, string action, ActionSettings settings)
        {
            return actionProvider.ExecuteFunction(action, settings, "action") as ActionFunctionResult;
        }

        public static void AddActionFunction(this IActionProvider actionProvider, string action, ActionFunction func)
        {
            actionProvider.Functions.Add("action_" + action, (settings) =>
            {
                return func(settings as ActionSettings);
            });
        }

        public static void AddActionFunction(this IActionProvider actionProvider, ActionFunction func)
        {
            actionProvider.AddActionFunction("default", func);
        }

        public static void AddSingleResultActionFunction(this IActionProvider actionProvider, string name, SingleResultActionFunction func)
        {
            actionProvider.Functions.Add("action_" + name, (settings) =>
            {
                var ar = func(settings as ActionSettings);
                var afr = new ActionFunctionResult();
                afr.RanSuccessfully = ar.RanSuccessfully;
                afr.FunctionException = ar.ExecutionException;
                afr.Message = ar.Message;
                afr.ActionResults.Add("default", ar);
                return afr;
            });
        }

        public static void AddSingleResultActionFunction(this IActionProvider actionProvider, SingleResultActionFunction func)
        {
            actionProvider.AddSingleResultActionFunction("default", func);
        }

    }
}

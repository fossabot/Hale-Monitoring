namespace Hale.Lib.Modules
{
    using System;

    public static class ModuleProviderExtensions
    {
        public static ModuleResultSet ExecuteFunction(
            this IModuleProviderBase moduleProvider,
            string function,
            ModuleSettingsBase settings,
            string functionType = "function")
        {
            string functionPrefixed = $"{functionType}_{function}";

            if (!moduleProvider.Functions.ContainsKey(functionPrefixed))
            {
                var functionTypeCap = functionType.ToUpper()[0] + functionType.Substring(1);
                return new ModuleResultSet()
                {
                    FunctionException = new Exception($"{functionTypeCap} \"{function}\" not found!"),
                    Message = $"{functionTypeCap} \"{function}\" not found!",
                    RanSuccessfully = false
                };
            }

            try
            {
                return moduleProvider.Functions[functionPrefixed].Invoke(settings);
            }
            catch (Exception x)
            {
                return new ModuleResultSet()
                {
                    FunctionException = x,
                    Message = $"Error executing {functionType} \"{function}\": {x.Message}.",
                    RanSuccessfully = false
                };
            }
        }
    }
}

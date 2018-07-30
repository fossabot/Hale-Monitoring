namespace Hale.Lib.Utilities
{
    using System;

    [Serializable]
    public class CriticalServiceProviderNotFoundException : Exception
    {
        public CriticalServiceProviderNotFoundException(Type t)
            : base($"Critical service <{t.FullName}> has not been provided.")
        {
        }
    }
}

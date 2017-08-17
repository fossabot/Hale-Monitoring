namespace Hale.Alert
{
    using System;

    public class HaleAlertInitializeException : HaleAlertException
    {
        public HaleAlertInitializeException()
            : base("Canot initialize Hale Alert module.")
        {
        }

        public HaleAlertInitializeException(Exception innerException)
            : base("Canot initialize Hale Alert module.", innerException)
        {
        }
    }
}

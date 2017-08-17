namespace Hale.Alert
{
    using System;

    [Serializable]
    public class HaleAlertException : Exception
    {
        public HaleAlertException()
            : base("Unknown Hale Alert exception.")
        {
        }

        public HaleAlertException(string message)
            : base(message)
        {
        }

        public HaleAlertException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public HaleAlertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

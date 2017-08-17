namespace Hale.Modules
{
    using System;

    public partial class ServiceModule
    {
        [Serializable]
        private class ServiceCommandException : Exception
        {
            public ServiceCommandException()
                : base()
            {
            }

            public ServiceCommandException(string m)
                : base(m)
            {
            }

            public ServiceCommandException(string m, Exception x)
                : base(m, x)
            {
            }
        }
    }
}

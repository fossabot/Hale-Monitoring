#if NETSTANDARD
using System;

namespace Hale.Lib.Facades
{
    public class ServiceBase: IDisposable
    {
        public string ServiceName { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void OnStart(string[] args)
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
#endif
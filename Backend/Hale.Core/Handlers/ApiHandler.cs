using Microsoft.Owin.Hosting;
using System;
using NLog;
using Hale.Lib.Utilities;
using Hale.Core.Config;

namespace Hale.Core.Handlers
{
    internal partial class ApiHandler
    {
        private readonly Logger _log;
        private readonly CoreConfig.ApiSection _apiSection;

        public ApiHandler()
        {
            _log = LogManager.GetCurrentClassLogger();
            _apiSection = ServiceProvider.GetServiceCritical<CoreConfig>().Api;

            TryToStartListening();
        }

        private void TryToStartListening()
        {
            string url = GetApiUri();
            try
            {
                WebApp.Start<Startup>(url);
                _log.Info($"API listening at \"{url}\".");
            }
            catch (Exception x)
            {
                _log.Error($"Could not start listening on \"{url}\": {x}");
            }
        }

        private string GetApiUri()
        {
            return new UriBuilder(_apiSection.Scheme, _apiSection.Host, _apiSection.Port).ToString();
        }
    }
}
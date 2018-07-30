namespace Hale.Core.Handlers
{
    using System;
    using Hale.Core.Config;
    using Hale.Lib.Utilities;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using NLog;

    internal partial class ApiHandler
    {
        private readonly Logger log;
        private readonly CoreConfig.ApiSection apiSection;

        public ApiHandler()
        {
            this.log = LogManager.GetCurrentClassLogger();
            this.apiSection = ServiceProvider.GetServiceCritical<CoreConfig>().Api;

            this.TryToStartListening();
        }

        private void TryToStartListening()
        {
            string url = this.GetApiUri();
            try
            {
                new WebHostBuilder()
                   .UseKestrel()
                   .UseUrls(url)
                   .UseStartup<Startup>();
                this.log.Info($"API listening at \"{url}\".");
            }
            catch (Exception x)
            {
                this.log.Error($"Could not start listening on \"{url}\": {x}");
            }
        }

        private string GetApiUri()
        {
            return new UriBuilder(this.apiSection.Scheme, this.apiSection.Host, this.apiSection.Port).ToString();
        }
    }
}
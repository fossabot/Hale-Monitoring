using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using System;
using NLog;
using Swashbuckle.Application;
using Hale.Lib.Utilities;
using Hale.Core.Config;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Cors;
using System.Reflection;

namespace Hale.Core.Handlers
{
    internal partial class ApiHandler
    {
        private partial class Startup
        {

            private static ILogger _log = LogManager.GetLogger("Hale.Core.OwinStartup");

            public void Configuration(IAppBuilder appBuilder)
            {
                var config = GetHttpConfiguration();
                ConfigureFrontend(appBuilder);

                appBuilder
                    .UseCors(CorsOptions.AllowAll)
                    .UseCookieAuthentication(new CookieAuthenticationOptions()
                    {
                        AuthenticationMode = AuthenticationMode.Active,
                        AuthenticationType = "HaleCoreAuth",
                        CookieHttpOnly = true,
                        CookieSecure = CookieSecureOption.SameAsRequest,
                        CookieName = "HaleCoreAuth",
                    })
                    .UseWebApi(config);
            }

            private static HttpConfiguration GetHttpConfiguration()
            {
                HttpConfiguration config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();
                config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                config.Filters.Add(new Utils.ExceptionHandlingAttribute());
                config.EnableSwagger(c => c.SingleApiVersion("v1", "Hale.Core Api v1")).EnableSwaggerUi();

                return config;
            }

            private void ConfigureFrontend(IAppBuilder app)
            {
                var _api = ServiceProvider.GetServiceCritical<System.Configuration.Configuration>().Api();
                if (String.IsNullOrEmpty(_api.FrontendRoot) || !Directory.Exists(_api.FrontendRoot))
                    return;

                var pfs = new PhysicalFileSystem(_api.FrontendRoot);
                var fso = new FileServerOptions()
                {
                    EnableDefaultFiles = true,
                    FileSystem = pfs,
                    EnableDirectoryBrowsing = false,
                };

#if DEBUG
                fso.EnableDirectoryBrowsing = true;
#endif
                app.UseFileServer(fso);
                _log.Info($"Serving static content from '{Path.GetFullPath(_api.FrontendRoot)}'.");


            }

            private static string GetAssemblyPath()
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
        }

        
    }
}
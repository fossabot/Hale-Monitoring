using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
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

namespace Hale.Core.Handlers
{
    internal class ApiHandler
    {
        private readonly Logger _log;
        private readonly ApiSection _apiSection;

        public ApiHandler()
        {
            _log = LogManager.GetCurrentClassLogger();
            _apiSection = ServiceProvider.GetServiceCritical<System.Configuration.Configuration>().Api();

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

        // ReSharper disable once ClassNeverInstantiated.Local
        private partial class Startup
        {

            // Hack: Since this is the (only?) easiest way to pass a parameter to the startup class -NM
            [ThreadStatic]

            private static string _baseUrl;
            public static string BaseUrl { get { return _baseUrl; } set { _baseUrl = value; } }

            private static ILogger _log = LogManager.GetLogger("Hale.Core.OwinStartup");

            // ReSharper disable once UnusedMember.Local
            public void Configuration(IAppBuilder appBuilder)
            {
                HttpConfiguration config = new HttpConfiguration();

                ConfigureRoutes(config);
                ConfigureFrontend(appBuilder);
                ConfigureJson(config);
                ConfigureExceptionHandling(config);
                ConfigureAuth(appBuilder);
                ConfigureCors(appBuilder);
                ConfigureSwagger(config);

                appBuilder.UseWebApi(config);


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

            private static void ConfigureCors(IAppBuilder appBuilder)
            {
                appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            }

            private static void ConfigureExceptionHandling(HttpConfiguration config)
            {
                config.Filters.Add(new Utils.ExceptionHandlingAttribute());
            }

            private static void ConfigureRoutes(HttpConfiguration config)
            {
                config.MapHttpAttributeRoutes();
                
                // SA: No HTTP-route template needed as we only use attribute based routing. 
            }

            private static void ConfigureJson(HttpConfiguration config)
            {
                // set the default json formatting to camelCase
                config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                    new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

                // add JsonAPI deserializer
                config.Formatters.Add(new Formatting.JsonApiMediaTypeFormatter(_baseUrl));
            }

            private static void ConfigureSwagger(HttpConfiguration config)
            {
                config.EnableSwagger(c =>
                {
                    c.IncludeXmlComments(
                        GetAssemblyPath() + "\\WebApiSwagger.XML");
                    c.SingleApiVersion("v1", "Version 1 of the Hale-Core API");
                })
                .EnableSwaggerUi();
            }

            private static string GetAssemblyPath()
            {
                return
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
                    );
            }

            private void ConfigureAuth(IAppBuilder app)
            {
                app.UseCookieAuthentication(new CookieAuthenticationOptions()
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AuthenticationType = "HaleCoreAuth",
                    CookieHttpOnly = true,
                    CookieSecure = CookieSecureOption.SameAsRequest,
                    CookieName = "HaleCoreAuth",
                });
            }
        }

        
    }
}
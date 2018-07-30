namespace Hale.Core.Handlers
{
    using System.IO;
    using System.Reflection;
    using System.Web.Http;
    using Hale.Core.Config;
    using Hale.Core.Data.Contexts;
    using Hale.Lib.Utilities;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.StaticFiles;
    using Newtonsoft.Json.Serialization;
    using NLog;
    using Owin;
    using Swashbuckle.Application;
    using Microsoft.EntityFrameworkCore;

    internal partial class ApiHandler
    {
        internal class Startup
        {
            private static ILogger log = LogManager.GetLogger("Hale.Core.OwinStartup");

            public void Configuration(IAppBuilder appBuilder)
            {
                var config = GetHttpConfiguration();
                this.ConfigureFrontend(appBuilder);

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

            private static string GetAssemblyPath()
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            }

            private void ConfigureFrontend(IAppBuilder app)
            {
                var api = Lib.Utilities.ServiceProvider.GetServiceCritical<CoreConfig>().Api;
                if (string.IsNullOrEmpty(api.FrontendRoot) || !Directory.Exists(api.FrontendRoot))
                {
                    return;
                }

                var pfs = new PhysicalFileSystem(api.FrontendRoot);
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
                log.Info($"Serving static content from '{Path.GetFullPath(api.FrontendRoot)}'.");

            }

            private void ConfigureServices(IServiceCollection services)
            {
                var db = Lib.Utilities.ServiceProvider.GetServiceCritical<CoreConfig>().Database;
                switch (db.Type)
                {
                    case DatabaseType.SqlServer:
                        services.AddDbContext<HaleDBContext>(o => o.UseSqlServer(
                            db.ConnectionString,
                            x => x.MigrationsAssembly("Hale.Core.Data")));
                        break;

                    case DatabaseType.PostgreSQL:
                        services.AddDbContext<HaleDBContext>(o => o.UseNpgsql(
                            db.ConnectionString,
                            x => x.MigrationsAssembly("Hale.Core.Data")));
                        break;
                }
            }
        }
    }
}
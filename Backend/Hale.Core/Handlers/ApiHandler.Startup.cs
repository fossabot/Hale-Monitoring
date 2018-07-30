namespace Hale.Core.Handlers
{
    using System.IO;
    using System.Reflection;
    using Hale.Core.Config;
    using Hale.Core.Data.Contexts;
    using Hale.Lib.Utilities;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Serialization;
    using NLog;
    //using Swashbuckle.Application;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.FileProviders;

    internal partial class ApiHandler
    {
        internal class Startup
        {
            private static ILogger log = LogManager.GetLogger("Hale.Core.OwinStartup");

            public void Configuration(IApplicationBuilder appBuilder)
            {
                // var config = GetHttpConfiguration();
                this.ConfigureFrontend(appBuilder);

                appBuilder
                    .UseCors(b => b.AllowAnyOrigin());
            }

            /*
            private static HttpConfiguration GetHttpConfiguration()
            {
                //HttpConfiguration config = new HttpConfiguration();

                //config.MapHttpAttributeRoutes();
                //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //config.Filters.Add(new Utils.ExceptionHandlingAttribute());
                //config.EnableSwagger(c => c.SingleApiVersion("v1", "Hale.Core Api v1")).EnableSwaggerUi();

                return config;
            }
            */

            private static string GetAssemblyPath()
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            }

            private void ConfigureFrontend(IApplicationBuilder app)
            {
                var api = Lib.Utilities.ServiceProvider.GetServiceCritical<CoreConfig>().Api;
                if (string.IsNullOrEmpty(api.FrontendRoot) || !Directory.Exists(api.FrontendRoot))
                {
                    return;
                }

                var fso = new FileServerOptions()
                {
                    EnableDefaultFiles = true,
                    FileProvider = new PhysicalFileProvider(api.FrontendRoot),
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

                services.AddMvc();
                services.AddAuthentication();
            }
        }
    }
}
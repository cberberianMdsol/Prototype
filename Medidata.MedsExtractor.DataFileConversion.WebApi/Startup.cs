using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppStatus.NetCore;
using AppVersion.NetCore;
using Autofac;
using Medidata.MAuth.AspNetCore;
using Medidata.MDLogging;
using Medidata.MDLogging.NetCore;
using Medidata.MedsExtractor.DataFileConversion.WebApi.Config;
using Medidata.SmokeTests;
using Medidata.SmokeTests.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;


namespace Medidata.MedsExtractor.DataFileConversion.WebApi
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static readonly Func<HttpRequest, string, bool> BypassService = (request, serviceName) =>
            !Configuration
                .GetSection($"AppSettings:{serviceName}:EnabledPaths")
                .AsEnumerable<PathString>()
                .Any(path => request.Path.StartsWithSegments(path));
        private static readonly Func<HttpRequest, bool> BypassZipkin = (request) => BypassService(request, "Zipkin");
        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("AutofacConfig.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.UseLogging(GetComponentInformation());
            services.AddMvc();
            services.Configure<AppConfiguration>(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IOptions<AppConfiguration> appSettingsAccessor)
        {

            loggerFactory.AddSerilog();
            loggerFactory.AddConsole(Configuration.GetSection("AppSettings:Logging"));
            loggerFactory.AddDebug();

            var appConfiguration = appSettingsAccessor.Value;
            var logger = serviceProvider.GetService<IMDLogger<Startup>>();

            logger.Info("***Start MedsExtractor DataFileConversion Service***");
            var appConfigurationAppSettings = appConfiguration.AppSettings;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                var configOptions = new MAuthMiddlewareOptions
                {
                                    ApplicationUuid = appConfigurationAppSettings.MAuth.Uuid,
                                    PrivateKey = appConfigurationAppSettings.MAuth.PrivateKey,
                                    HideExceptionsAndReturnUnauthorized = appConfigurationAppSettings.MAuth.HideExceptionsAndReturnForbidden.HasValue ? appConfigurationAppSettings.MAuth.HideExceptionsAndReturnForbidden.Value : true,
                                    MAuthServiceUrl = new Uri(appConfigurationAppSettings.MAuth.ServiceUrl),
                };
                app.UseMAuthAuthentication(configOptions);
            }

            app.UseAppStatus();
            app.UseMvc();
            app.ApplicationServices.GetService<IApplicationLifetime>()
                .RegisterZipkinOnStarted(appConfigurationAppSettings, loggerFactory)
                .RegisterZipkinOnStopped();

            app.UseStaticFiles()
                .UseAppStatus()
                .UseAppVersion()
                .UseZipkin(Configuration["SubsystemName"], BypassZipkin)
                .UseSmokeTests(smokeTestMiddlewareOptions => smokeTestMiddlewareOptions.TestResolver = serviceType => serviceProvider.GetService(serviceType) as ISmokeTest);



        }

        

        private static bool Bypass(HttpRequest request, IList<string> whitelist)
        {
            var bypass = !request.Path.HasValue ||
                         whitelist.Any(uri => request.Path.Value.StartsWith(uri, StringComparison.InvariantCultureIgnoreCase));

//            if (bypass)
//                MDLogger.Debug($"Will bypass {request.Path.Value}");

            return bypass;
        }

        private static IComponentInformation GetComponentInformation()
        {
            var version = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            return new ComponentInformation { Name = "Medidata.MedsExtractor.DataFileConversion", Version = version };
        }
    }
}

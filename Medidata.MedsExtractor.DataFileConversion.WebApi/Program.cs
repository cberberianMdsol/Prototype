using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Medidata.MedsExtractor.DataFileConversion.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                    {
                        services.AddAutofac();
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Info { Title = "Meds Extractor Datafile Conversion API", Version = "v1" });
                        });
                    }
                 )
                .UseStartup<Startup>()
                .Build();
    }
}

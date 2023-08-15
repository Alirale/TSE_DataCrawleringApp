using System.Text.Json.Serialization;
using CrystalQuartz.AspNetCore;
using Quartz.Impl;
using TSE_DataCrawler.Filters;
using TSE_DataCrawler.ServicesExtensions;

namespace TSE_DataCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });

            var generalConfigs = GeneralConfigsSetting.GetGeneralSettings(config);

            builder.Services.AddControllers(options => { options.Filters.Add<ApiExceptionFilterAttribute>(); })
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            
            builder.WebHost.UseUrls(generalConfigs.Url);

            builder.ConfigKestrel(generalConfigs.Port);

            builder.Services.AddServices(config);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerApiVersion(builder.Environment);

            builder.Services.ConfigureCors();

            builder.Services.AddMvc();

            builder.Services.AddControllers();


            var app = builder.Build();
            
            app.UseCors("AllowOrigin");

            app.MapControllers();
            
            app.UseCrystalQuartz(() => new StdSchedulerFactory().GetScheduler().Result);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
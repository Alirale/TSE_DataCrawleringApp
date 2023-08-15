using System.Net;
using Application;
using Application.BackgroundJobs;
using Domain.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerGen;
using TSE_DataCrawler.Options;

namespace TSE_DataCrawler.ServicesExtensions
{
    public static class ServicesExtensions
    {

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", configurePolicy =>
                {
                    configurePolicy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }


        public static WebApplicationBuilder ConfigKestrel(this WebApplicationBuilder builder, int applicationPort)
        {
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ConfigureHttpsDefaults(listenOptions =>
                {
                    serverOptions.Listen(IPAddress.Any, applicationPort, options =>
                    {
                        options.UseConnectionLogging();
                        options.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                });
            });
            return builder;
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices();
            services.AddInfrastructureServices();
            services.AddHttpClient();

            services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler";
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 10; });
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.AddHostedService<JobConfig>();

            services.AddTransient<TseRequestCrawlJob>();


            var generalConfigs = GeneralConfigsSetting.GetGeneralSettings(configuration);
            services.AddSingleton<GeneralSettings>(_ => new GeneralSettings()
            {
                Url = generalConfigs.Url,
                TSE_MarketCrawlUrl = generalConfigs.TSE_MarketCrawlUrl,
                Port = generalConfigs.Port
            });

            var rabbitMqSettings = GeneralConfigsSetting.GetRabbitMqSettings(configuration);
            services.AddSingleton<RabbitMqConfig>(_ => new RabbitMqConfig()
            {
                Password = rabbitMqSettings.Password,
                UserName = rabbitMqSettings.UserName,
                Port = rabbitMqSettings.Port,
                HostName = rabbitMqSettings.HostName,
                VirtualHost = rabbitMqSettings.VirtualHost
            });
        }

        public static IServiceCollection AddSwaggerApiVersion(this IServiceCollection services,
            IHostEnvironment environment)
        {
            services.AddApiVersioning(option =>
            {
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = ApiVersion.Default;
                option.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            //if (environment.IsDevelopment())
            //{
            services.AddSwaggerGen(
                options =>
                {
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] { }
                            }
                    });
                }
                );
            //}
            return services;
        }

    }
}
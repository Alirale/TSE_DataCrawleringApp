using Application.Interfaces;
using Application.Services;
using Infrastructure.RabbitMQService;
using Infrastructure.Repository;
using Infrastructure.WebService;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ISymbolDataAccess, SymbolDataAccess>();
            services.AddSingleton<ITseMarketService, TseMarketService>();
            services.AddSingleton<RabbitMqPublisher>();
            services.AddSingleton<IEventSenderProducer, EventSenderProducer>();
            services.AddSingleton<ISymbolService, SymbolService>();
            return services;
        }
    }
}
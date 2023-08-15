using Domain.Models;

namespace TSE_DataCrawler.ServicesExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GeneralConfigsSetting
    {
        public static GeneralSettings GetGeneralSettings(IConfiguration configuration)
        {
            var generalSettings = configuration.GetSection("GeneralSettings").Get<GeneralSettings>();
            if (generalSettings is null) throw new NullReferenceException();
            generalSettings.Url += generalSettings.Port;
            return generalSettings;
        }
        public static RabbitMqConfig GetRabbitMqSettings(IConfiguration configuration)
        {
            var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>();
            if (rabbitMqConfig is null) throw new NullReferenceException();
            return rabbitMqConfig;
        }
    }
}
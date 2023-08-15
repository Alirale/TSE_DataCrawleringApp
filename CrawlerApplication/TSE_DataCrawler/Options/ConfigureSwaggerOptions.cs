using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TSE_DataCrawler.Options
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                try
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Versioning API",
                Version = description.ApiVersion.ToString()
            };
            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }
            return info;
        }
    }
}
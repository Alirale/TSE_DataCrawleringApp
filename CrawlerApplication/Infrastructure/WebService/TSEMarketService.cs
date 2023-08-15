using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace Infrastructure.WebService
{
    public class TseMarketService : ITseMarketService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TseMarketService> _logger;
        private readonly GeneralSettings _generalSettings;

        public TseMarketService(IHttpClientFactory httpClientFactory, ILogger<TseMarketService> logger,
            GeneralSettings generalSettings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _generalSettings = generalSettings;
        }

        public async Task<string> GetMarketWatchDataAsync()
        {
            try
            {
                var url = _generalSettings.TSE_MarketCrawlUrl;
                if (url.IsEmpty() || !IsHttpUrl(url))
                    throw new HttpRequestException("URL not valid");

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Origin", "http://main.tsetmc.com");
                client.DefaultRequestHeaders.Add("Referer", "http://main.tsetmc.com/");
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }

                _logger.LogError($"HTTP request failed with status code: {response.StatusCode}");
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, e.Message);
                throw;
            }
        }


        private static bool IsHttpUrl(string url)
        {
            try
            {
                return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                       && uriResult.Scheme == Uri.UriSchemeHttp;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
namespace Application.Interfaces;

public interface ITseMarketService
{
    public Task<string> GetMarketWatchDataAsync();
}
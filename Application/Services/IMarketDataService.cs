using Domain.Entities;

namespace Application.Services
{
    public interface IMarketDataService
    {
        public Task<List<Symbol>> GetMarketData();
        public Task<List<Symbol>> GetChangedData(List<Symbol> currentData, List<Symbol> previousData);
    }
}
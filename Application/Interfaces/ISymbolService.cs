using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISymbolService
    {
        public Task<List<Symbol>> GetSymbols();
    }
}
using System.Data;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISymbolDataAccess
    {
        public Task<List<Symbol>> GetSymbols();
        public Task<bool> AddSymbols(DataTable symbols);
        public Task<bool> UpdateSymbols(DataTable symbols);
        public Task<bool> DeleteAllSymbols();
    }
}
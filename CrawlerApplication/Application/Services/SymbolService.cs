using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class SymbolService : ISymbolService
    {
        private readonly ISymbolDataAccess _symbolDataAccess;

        public SymbolService( ISymbolDataAccess usersDataAccess)
        {
            _symbolDataAccess = usersDataAccess;
        }


        public async Task<List<Symbol>> GetSymbols()
        {
            return await _symbolDataAccess.GetSymbols();
        }
    }
}
using Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using Domain.Entities;

namespace Infrastructure.Repository
{
    public class SymbolDataAccess: ISymbolDataAccess
    {
        private readonly ILogger<SymbolDataAccess> _logger;
        private readonly string? _connectionString;

        public SymbolDataAccess(IConfiguration config, ILogger<SymbolDataAccess> logger)
        {
            _connectionString = config["connection"];
            _logger = logger;
        }
        public async Task<List<Symbol>> GetSymbols()
        {
            try
            {
                await using var sqlConnection = new SqlConnection(_connectionString);
                var result =( await
                    (sqlConnection.QueryAsync<Symbol>("Symbol_GetSymbols",
                        commandType: CommandType.StoredProcedure))).ToList();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAllSymbols()
        {
            try
            {
                await using var sqlConnection = new SqlConnection(_connectionString);
                var result = await
                    (sqlConnection.ExecuteAsync("Symbol_DeleteSymbols",
                        commandType: CommandType.StoredProcedure));
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<bool> AddSymbols(DataTable symbols)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var cmd = new SqlCommand("AddSymbols", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("@SymbolData", SqlDbType.Structured)
                {
                    Value = symbols,
                    TypeName = "dbo.AddSymbolTableType"
                };
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<bool> UpdateSymbols(DataTable updatedSymbols)
        {
            try
            {
                await using var dbConnection = new SqlConnection(_connectionString);
                dbConnection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@UpdatedSymbols", updatedSymbols.AsTableValuedParameter("dbo.UpdatedSymbolType"));

                await dbConnection.ExecuteAsync("UpdateSymbolData", parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}

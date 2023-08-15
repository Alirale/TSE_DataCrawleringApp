using Newtonsoft.Json;
using System.Data;
using Domain.Entities;

namespace Application.Common;

public static class SerializationHelper
{
    public static string SerializeToJson<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    public static DataTable UpdateSymbolsModelToDataTable(List<Symbol> symbols)
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("SymbolISIN", typeof(string));
        dataTable.Columns.Add("lastTradedPrice", typeof(decimal));
        dataTable.Columns.Add("ClosingPrice", typeof(decimal));
        dataTable.Columns.Add("HighPrice", typeof(decimal));
        dataTable.Columns.Add("Value", typeof(decimal));
        dataTable.Columns.Add("Volume", typeof(decimal));
        dataTable.Columns.Add("Quantity", typeof(decimal));
        dataTable.Columns.Add("LowPrice", typeof(decimal));
        dataTable.Columns.Add("EPS", typeof(string));
        dataTable.Columns.Add("PE", typeof(string));
        dataTable.Columns.Add("YesterdayClosingPrice", typeof(decimal));
        dataTable.Columns.Add("FirstTradedPrice", typeof(decimal));
        foreach (var symbol in symbols)
        {
            dataTable.Rows.Add(
                symbol.SymbolISIN,
                symbol.lastTradedPrice,
                symbol.ClosingPrice,
                symbol.HighPrice,
                symbol.Value,
                symbol.Volume,
                symbol.Quantity,
                symbol.LowPrice,
                symbol.EPS,
                symbol.PE,
                symbol.YesterdayClosingPrice,
                symbol.FirstTradedPrice
            );
        }
        return dataTable;
    }
    public static DataTable AddSymbolsModelToDataTable(List<Symbol> symbols)
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("SymbolISIN", typeof(string));
        dataTable.Columns.Add("SymbolTitle", typeof(string));
        dataTable.Columns.Add("CompanyTitle", typeof(string));
        dataTable.Columns.Add("lastTradedPrice", typeof(decimal));
        dataTable.Columns.Add("ClosingPrice", typeof(decimal));
        dataTable.Columns.Add("HighPrice", typeof(decimal));
        dataTable.Columns.Add("Value", typeof(decimal));
        dataTable.Columns.Add("Volume", typeof(decimal));
        dataTable.Columns.Add("Quantity", typeof(decimal));
        dataTable.Columns.Add("LowPrice", typeof(decimal));
        dataTable.Columns.Add("EPS", typeof(string));
        dataTable.Columns.Add("PE", typeof(string));
        dataTable.Columns.Add("YesterdayClosingPrice", typeof(decimal));
        dataTable.Columns.Add("FirstTradedPrice", typeof(decimal));
        dataTable.Columns.Add("InsCode", typeof(string));
        foreach (var symbol in symbols)
        {
            dataTable.Rows.Add(
                symbol.SymbolISIN,
                symbol.SymbolTitle,
                symbol.CompanyTitle,
                symbol.lastTradedPrice,
                symbol.ClosingPrice,
                symbol.HighPrice,
                symbol.Value,
                symbol.Volume,
                symbol.Quantity,
                symbol.LowPrice,
                symbol.EPS,
                symbol.PE,
                symbol.YesterdayClosingPrice,
                symbol.FirstTradedPrice,
                symbol.InsCode
            );
        }
        return dataTable;
    }
}
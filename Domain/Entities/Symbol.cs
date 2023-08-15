namespace Domain.Entities;

public class Symbol
{
    public string? InsCode { get; set; }
    public string SymbolISIN { get; set; }
    public string SymbolTitle { get; set; }
    public string CompanyTitle { get; set; }
    public string? lastTradedPrice{ get; set; }
    public string? ClosingPrice{ get; set; }
    public string? HighPrice{ get; set; }
    public string? Value{ get; set; }
    public string? Volume { get; set; }
    public string? Quantity{ get; set; }
    public string? LowPrice{ get; set; }
    public string? EPS{ get; set; }
    public string? PE{ get; set; }
    public string? YesterdayClosingPrice { get; set; }
    public string? FirstTradedPrice { get; set; }
}
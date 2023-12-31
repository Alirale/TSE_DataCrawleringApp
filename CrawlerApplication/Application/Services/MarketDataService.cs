﻿using System.Data;
using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MarketDataService: IMarketDataService
    {
        private readonly ITseMarketService _tseMarketService;
        private readonly ILogger<MarketDataService> _logger;

        public MarketDataService(ITseMarketService tseMarketService, ILogger<MarketDataService> logger)
        {
            _tseMarketService = tseMarketService;
            _logger = logger;
        }

        public async Task<List<Symbol>> GetMarketData()
        {
            try
            {
                var rawMarketData =await _tseMarketService.GetMarketWatchDataAsync();
                if (!string.IsNullOrEmpty(rawMarketData))
                {
                    var serilizedSymbols = TseSymbolSerializer.DeSerilizeSymbols(rawMarketData);
                    //serilizedSymbols.ForEach(x=>x.ClosingPrice = (Convert.ToInt32(x.ClosingPrice) + new Random().Next(-100,100)).ToString());
                    return serilizedSymbols;
                }
                else
                {
                    throw new NoNullAllowedException("Data fetch Failed");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                throw;
            }
        }

        public async Task<List<Symbol>> GetChangedData(List<Symbol> currentData, List<Symbol> previousData)
        {
            if (previousData is null)
                return null;
            return (from currentSymbol in currentData
                let previousSymbol = previousData.FirstOrDefault(x => x.SymbolISIN == currentSymbol.SymbolISIN)
                where previousSymbol != null
                let symbolHasChangesData = false
                select new Symbol
                {
                    InsCode = null,
                    SymbolISIN = currentSymbol.SymbolISIN,
                    lastTradedPrice = (currentSymbol.lastTradedPrice == previousSymbol.lastTradedPrice) ? null : currentSymbol.lastTradedPrice,
                    ClosingPrice = (currentSymbol.ClosingPrice == previousSymbol.ClosingPrice) ? null : currentSymbol.ClosingPrice,
                    HighPrice = (currentSymbol.HighPrice == previousSymbol.HighPrice) ? null : currentSymbol.HighPrice,
                    Value = (currentSymbol.Value == previousSymbol.Value) ? null : currentSymbol.Value,
                    Volume = (currentSymbol.Volume == previousSymbol.Volume) ? null : currentSymbol.Volume,
                    Quantity = (currentSymbol.Quantity == previousSymbol.Quantity) ? null : currentSymbol.Quantity,
                    LowPrice = (currentSymbol.LowPrice == previousSymbol.LowPrice) ? null : currentSymbol.LowPrice,
                    EPS = (currentSymbol.EPS == previousSymbol.EPS) ? null : currentSymbol.EPS,
                    PE = (currentSymbol.PE == previousSymbol.PE) ? null : currentSymbol.PE,
                    YesterdayClosingPrice = (currentSymbol.YesterdayClosingPrice == previousSymbol.YesterdayClosingPrice) ? null : currentSymbol.YesterdayClosingPrice,
                    FirstTradedPrice = (currentSymbol.FirstTradedPrice == previousSymbol.FirstTradedPrice) ? null : currentSymbol.FirstTradedPrice
                }
                into symbolDiff
                where HasChanges(symbolDiff)
                select symbolDiff).ToList();
        }

        private static bool HasChanges(Symbol? previousSymbol)
        {
            if (previousSymbol == null)
                return true; 

            return previousSymbol.lastTradedPrice !=null ||
                   previousSymbol.ClosingPrice != null ||
                   previousSymbol.HighPrice != null ||
                   previousSymbol.Value != null ||
                   previousSymbol.Volume != null ||
                   previousSymbol.Quantity != null ||
                   previousSymbol.LowPrice != null ||
                   previousSymbol.EPS != null ||
                   previousSymbol.PE != null ||
                   previousSymbol.YesterdayClosingPrice != null ||
                   previousSymbol.FirstTradedPrice != null;
        }
    }
}
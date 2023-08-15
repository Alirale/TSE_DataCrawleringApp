using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using Application.Common;

namespace Application.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class TseRequestCrawlJob:IJob
    {
        private readonly IMarketDataService _marketDataService;
        private readonly ISymbolDataAccess _symbolDataAccess;
        private readonly IEventSenderProducer _eventSenderProducer;
        private readonly ILogger<TseRequestCrawlJob> _logger;
        public TseRequestCrawlJob(IMarketDataService marketDataService, ISymbolDataAccess symbolDataAccess, IEventSenderProducer eventSenderProducer, ILogger<TseRequestCrawlJob> logger)
        {
            _marketDataService = marketDataService;
            _symbolDataAccess = symbolDataAccess;
            _eventSenderProducer = eventSenderProducer;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var newMarketData = await _marketDataService.GetMarketData();
            
            //Get Last SymbolState From Db
            var previousSymbols =await _symbolDataAccess.GetSymbols();

            if (previousSymbols.Count != 0 )
            {
                //Get changed data
                var changedData = await _marketDataService.GetChangedData(newMarketData, previousSymbols);
                if (changedData.Count != 0)
                {
                    _logger.LogInformation("Number of changed data of Current Cycle : " + changedData.Count);
                    var changedDataToDataTable = SerializationHelper.UpdateSymbolsModelToDataTable(changedData);

                    //Send newDataTo Db
                    var saveSymbolsResult = await _symbolDataAccess.UpdateSymbols(changedDataToDataTable);

                    if (saveSymbolsResult)
                    {
                        //Send Event To Rabbit
                        _eventSenderProducer.PublishEvent(changedData);
                    }
                }
            }
            else
            {
                //Send newDataTo Db
                var addSymbolsDataTable = SerializationHelper.AddSymbolsModelToDataTable(newMarketData);
                await _symbolDataAccess.AddSymbols(addSymbolsDataTable);
            }
            //End of Cycle
        }

    }
}
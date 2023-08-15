using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMQService;

public class EventSenderProducer: IEventSenderProducer
{
    private readonly RabbitMqPublisher _rabbitMqPublisher;
    private readonly ILogger<EventSenderProducer> _logger;

    public EventSenderProducer(RabbitMqPublisher rabbitMqPublisher, ILogger<EventSenderProducer> logger)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
        _logger = logger;
    }

    public void PublishEvent(List<Symbol> model)
    {
        try
        {
            const string exchangeName = "TseData";

            var message = SerializationHelper.SerializeToJson(model);
            _rabbitMqPublisher.PublishMessage(exchangeName,message);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }
}
using Domain.Entities;

namespace Application.Interfaces;

public interface IEventSenderProducer
{
    void PublishEvent(List<Symbol> model);
}
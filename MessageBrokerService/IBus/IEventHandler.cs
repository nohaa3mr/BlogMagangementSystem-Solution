using MessageBrokerService.Events;

namespace MessageBrokerService.IBus
{
    public interface IEventHandler<in TEvent> where TEvent : Event
    {
         Task Handle(TEvent @event);
    }
    public interface IEventHandler { }
}

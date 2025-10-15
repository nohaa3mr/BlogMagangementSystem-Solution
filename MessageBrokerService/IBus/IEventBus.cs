using MessageBrokerService.Commands;
using MessageBrokerService.Events;

namespace MessageBrokerService.IBus
{
    public interface IEventBus 
    {
        public Task SendCommand<T>(T command) where T : Command;
        public Task Publish<T>(T @event) where T : Event;
        public Task Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}

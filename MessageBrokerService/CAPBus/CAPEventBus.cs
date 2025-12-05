using DotNetCore.CAP;
using MessageBrokerService.Commands;
using MessageBrokerService.Events;
using MessageBrokerService.IBus;
using MediatR;

namespace MessageBrokerService.CAPBus
{
    /// <summary>
    /// CAP-based implementation of IEventBus
    /// Note: CAP uses attribute-based subscriptions ([CapSubscribe]) rather than programmatic subscriptions.
    /// To subscribe to events, create classes that implement ICapSubscribe with [CapSubscribe("EventName")] attributes.
    /// For examples, see CAPEventHandlerAdapter or create handlers that implement IEventHandler and register them as CAP subscribers.
    /// </summary>
    public class CAPEventBus : IEventBus
    {
        private readonly ICapPublisher _capPublisher;
        private readonly IMediator _mediator;

        public CAPEventBus(ICapPublisher capPublisher, IMediator mediator)
        {
            _capPublisher = capPublisher;
            _mediator = mediator;
        }

        public async Task SendCommand<T>(T command) where T : Command
        {
            await _mediator.Send(command);
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            var eventName = typeof(T).Name;
            await _capPublisher.PublishAsync(eventName, @event);
        }

        public Task Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            // CAP uses attribute-based subscriptions via [CapSubscribe] attributes
            // This method is kept for interface compatibility but does not perform runtime registration
            // To subscribe to events with CAP:
            // 1. Create a class that implements ICapSubscribe
            // 2. Add a method with [CapSubscribe("EventName")] attribute
            // 3. Register the class in DI container
            // 
            // Example:
            // public class MyEventHandler : ICapSubscribe
            // {
            //     [CapSubscribe("MyEvent")]
            //     public async Task HandleAsync(MyEvent @event) { ... }
            // }
            
            return Task.CompletedTask;
        }
    }
}


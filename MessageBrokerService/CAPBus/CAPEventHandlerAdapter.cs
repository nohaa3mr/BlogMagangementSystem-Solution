using DotNetCore.CAP;
using MessageBrokerService.Events;
using MessageBrokerService.IBus;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokerService.CAPBus
{
    /// <summary>
    /// Helper class to bridge CAP subscriptions to IEventHandler implementations
    /// Usage: Create a concrete subscriber class for each event type
    /// </summary>
    public abstract class CAPEventSubscriber<TEvent> : ICapSubscribe where TEvent : Event
    {
        protected readonly IServiceProvider ServiceProvider;

        protected CAPEventSubscriber(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// This method should be overridden in derived classes with [CapSubscribe("EventName")] attribute
        /// Example:
        /// [CapSubscribe("MyEvent")]
        /// public override async Task HandleAsync(MyEvent @event) { ... }
        /// </summary>
        public abstract Task HandleAsync(TEvent @event);

        /// <summary>
        /// Helper method to get and invoke the IEventHandler for this event
        /// </summary>
        protected async Task InvokeHandlerAsync(TEvent @event)
        {
            using var scope = ServiceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IEventHandler<TEvent>>();
            if (handler != null)
            {
                await handler.Handle(@event);
            }
            else
            {
                throw new InvalidOperationException($"No IEventHandler<{typeof(TEvent).Name}> found in service provider");
            }
        }
    }
}


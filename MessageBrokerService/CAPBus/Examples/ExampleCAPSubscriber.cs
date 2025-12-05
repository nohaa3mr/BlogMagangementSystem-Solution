using DotNetCore.CAP;
using MessageBrokerService.Events;
using MessageBrokerService.IBus;

namespace MessageBrokerService.CAPBus.Examples
{
    /// <summary>
    /// Example: How to create a CAP subscriber for an event
    /// Replace "ExampleEvent" with your actual event class name
    /// </summary>
    
    // Option 1: Direct CAP subscriber (recommended)
    // Create a class that implements ICapSubscribe and use [CapSubscribe] attribute
    /*
    public class ExampleEventSubscriber : ICapSubscribe
    {
        private readonly IEventHandler<ExampleEvent> _handler;

        public ExampleEventSubscriber(IEventHandler<ExampleEvent> handler)
        {
            _handler = handler;
        }

        [CapSubscribe("ExampleEvent")] // Event name must match the class name
        public async Task HandleAsync(ExampleEvent @event)
        {
            await _handler.Handle(@event);
        }
    }
    */

    // Option 2: Standalone CAP subscriber without IEventHandler
    /*
    public class ExampleEventSubscriber : ICapSubscribe
    {
        [CapSubscribe("ExampleEvent")]
        public async Task HandleAsync(ExampleEvent @event)
        {
            // Your business logic here
            Console.WriteLine($"Received event: {@event.GetType().Name}");
            // Process the event...
        }
    }
    */

    // Don't forget to register your subscriber in Program.cs:
    // builder.Services.AddTransient<ExampleEventSubscriber>();
}


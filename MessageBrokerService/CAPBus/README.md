# CAP Library Integration Guide

This project now uses the **DotNetCore.CAP** library for message publishing and consuming with RabbitMQ.

## Configuration

### Prerequisites
1. **RabbitMQ** - Must be running and accessible (default: localhost:5672)
2. **SQL Server** - Required for CAP message storage (configured in appsettings.json)

### Setup

1. **Database Setup**: CAP will automatically create necessary tables on first run. Ensure your SQL Server connection string in `appsettings.json` is correct.

2. **RabbitMQ Configuration**: Update `appsettings.json` if your RabbitMQ instance uses different credentials:
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "Port": 5672
  }
}
```

## Usage

### Publishing Events

Use `IEventBus.Publish<T>()` method:

```csharp
public class MyController : ControllerBase
{
    private readonly IEventBus _eventBus;

    public MyController(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSomething()
    {
        var myEvent = new MyEvent { /* properties */ };
        await _eventBus.Publish(myEvent);
        return Ok();
    }
}
```

### Consuming Events

CAP uses **attribute-based subscriptions**. Create a subscriber class:

```csharp
using DotNetCore.CAP;
using MessageBrokerService.Events;

public class MyEventSubscriber : ICapSubscribe
{
    [CapSubscribe("MyEvent")] // Event name must match your Event class name
    public async Task HandleAsync(MyEvent @event)
    {
        // Your business logic here
        Console.WriteLine($"Received: {@event.GetType().Name}");
        // Process the event...
    }
}
```

**Important**: Register your subscriber in `Program.cs`:
```csharp
builder.Services.AddTransient<MyEventSubscriber>();
```

### Using with IEventHandler Pattern

If you have existing `IEventHandler<T>` implementations, create a bridge:

```csharp
using DotNetCore.CAP;
using MessageBrokerService.Events;
using MessageBrokerService.IBus;

public class MyEventSubscriber : ICapSubscribe
{
    private readonly IEventHandler<MyEvent> _handler;

    public MyEventSubscriber(IEventHandler<MyEvent> handler)
    {
        _handler = handler;
    }

    [CapSubscribe("MyEvent")]
    public async Task HandleAsync(MyEvent @event)
    {
        await _handler.Handle(@event);
    }
}
```

## Key Features

- **Reliable Messaging**: CAP ensures message delivery and persistence
- **Transaction Support**: Messages are stored in SQL Server before publishing
- **Retry Mechanism**: Failed messages are automatically retried (configured: 5 retries, 60s interval)
- **Monitoring**: Optional CAP Dashboard available (uncomment in Program.cs)

## Migration Notes

- The old `RabbitMQBus` implementation has been replaced with `CAPEventBus`
- `IEventBus.Subscribe<T, TH>()` method is kept for interface compatibility but does not perform runtime registration
- All subscriptions must use `[CapSubscribe]` attributes on subscriber classes
- Commands continue to use MediatR as before


# Quick Start Guide - CAP Library Usage

## 1. Publishing an Event from an Endpoint

```csharp
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    private readonly IEventBus _eventBus;

    public MyController(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        // 1. Perform your business logic
        var entityId = Guid.NewGuid();
        // ... save to database ...

        // 2. Publish event
        var @event = new MyEvent
        {
            EntityId = entityId,
            // ... other properties
        };
        
        await _eventBus.Publish(@event);
        
        return Ok(new { Id = entityId });
    }
}
```

## 2. Creating a Subscriber

```csharp
using DotNetCore.CAP;
using MessageBrokerService.Events;

public class MyEventSubscriber : ICapSubscribe
{
    private readonly ILogger<MyEventSubscriber> _logger;

    public MyEventSubscriber(ILogger<MyEventSubscriber> logger)
    {
        _logger = logger;
    }

    // Event name must match the Event class name exactly
    [CapSubscribe("MyEvent")]
    public async Task HandleAsync(MyEvent @event)
    {
        _logger.LogInformation("Received event: {EntityId}", @event.EntityId);
        
        // Your processing logic here
        // This runs asynchronously and won't block the API response
        
        await Task.CompletedTask;
    }
}
```

## 3. Registering the Subscriber

In `Program.cs`:

```csharp
builder.Services.AddTransient<MyEventSubscriber>();
```

## 4. Complete Example Flow

### Step 1: Define Event
```csharp
// Events/OrderCreatedEvent.cs
public class OrderCreatedEvent : Event
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
}
```

### Step 2: Publish Event
```csharp
// Controllers/OrderController.cs
[HttpPost("orders")]
public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
{
    var orderId = Guid.NewGuid();
    // ... save order ...

    await _eventBus.Publish(new OrderCreatedEvent
    {
        OrderId = orderId,
        Amount = request.Amount,
        CustomerEmail = request.Email
    });

    return Ok(new { OrderId = orderId });
}
```

### Step 3: Create Subscribers
```csharp
// Subscribers/EmailSubscriber.cs
public class EmailSubscriber : ICapSubscribe
{
    [CapSubscribe("OrderCreatedEvent")]
    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        // Send confirmation email
        await _emailService.SendOrderConfirmation(@event.CustomerEmail, @event.OrderId);
    }
}

// Subscribers/InventorySubscriber.cs
public class InventorySubscriber : ICapSubscribe
{
    [CapSubscribe("OrderCreatedEvent")]
    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        // Update inventory
        await _inventoryService.ReserveItems(@event.OrderId);
    }
}
```

### Step 4: Register All Subscribers
```csharp
// Program.cs
builder.Services.AddTransient<EmailSubscriber>();
builder.Services.AddTransient<InventorySubscriber>();
```

## 5. Testing the Flow

1. **Start RabbitMQ** (if not running):
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

2. **Ensure SQL Server is running** (for CAP message storage)

3. **Call your API endpoint**:
   ```bash
   POST /api/orders
   {
     "amount": 100.00,
     "email": "customer@example.com"
   }
   ```

4. **Check logs** - you should see:
   - Order created message
   - Email sent message
   - Inventory updated message

## 6. Common Use Cases

### Use Case: Send Notification
```csharp
[CapSubscribe("UserRegisteredEvent")]
public async Task SendWelcomeEmail(UserRegisteredEvent @event)
{
    await _emailService.SendWelcomeEmail(@event.Email);
}
```

### Use Case: Update Cache
```csharp
[CapSubscribe("UserUpdatedEvent")]
public async Task InvalidateCache(UserUpdatedEvent @event)
{
    await _cache.RemoveAsync($"user:{@event.UserId}");
}
```

### Use Case: Track Analytics
```csharp
[CapSubscribe("ProductViewedEvent")]
public async Task TrackView(ProductViewedEvent @event)
{
    await _analytics.TrackEvent("product_viewed", new { @event.ProductId });
}
```

### Use Case: Integrate with External Service
```csharp
[CapSubscribe("OrderCreatedEvent")]
public async Task SyncToCRM(OrderCreatedEvent @event)
{
    await _crmService.CreateOrder(@event.OrderId, @event.Amount);
}
```

## 7. Important Notes

✅ **Event Name Must Match**: The `[CapSubscribe("EventName")]` must exactly match your Event class name

✅ **Multiple Subscribers**: Multiple subscribers can listen to the same event

✅ **Async Processing**: Subscribers run asynchronously and don't block the API response

✅ **Automatic Retry**: Failed messages are automatically retried (configured: 5 retries)

✅ **Message Persistence**: All messages are stored in SQL Server before publishing

✅ **Order Guarantee**: CAP maintains message order within the same event type

## 8. Viewing Messages (CAP Dashboard)

Uncomment in `Program.cs`:
```csharp
options.UseDashboard();
```

Then access: `http://localhost:5000/cap`

## Next Steps

- See `USE_CASES.md` for detailed use case scenarios
- Review example controllers in `UseCases/Controllers/`
- Review example subscribers in `UseCases/Subscribers/`
- Review example events in `UseCases/Events/`


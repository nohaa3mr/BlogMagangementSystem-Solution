# CAP Library Use Cases - Complete Guide

This document provides comprehensive examples of using the CAP library for event-driven messaging in your application.

## Table of Contents
1. [Overview](#overview)
2. [Event Flow Architecture](#event-flow-architecture)
3. [Use Cases](#use-cases)
4. [Implementation Examples](#implementation-examples)
5. [Best Practices](#best-practices)

## Overview

The CAP library enables **asynchronous event-driven communication** between different parts of your application or between microservices. Events are published once and can be consumed by multiple subscribers independently.

### Key Benefits:
- **Decoupling**: Publishers don't need to know about subscribers
- **Scalability**: Multiple subscribers can process events independently
- **Reliability**: CAP ensures message delivery with retry mechanisms
- **Transaction Support**: Events are persisted in SQL Server before publishing
- **Event Ordering**: CAP maintains message order

## Event Flow Architecture

```
┌─────────────────┐
│  API Controller │
│  (Publisher)    │
└────────┬────────┘
         │
         │ Publish Event
         ▼
┌─────────────────┐
│   IEventBus     │
│  (CAPEventBus)  │
└────────┬────────┘
         │
         │ Store in SQL Server + Publish to RabbitMQ
         ▼
┌─────────────────┐
│    RabbitMQ     │
│  Message Queue  │
└────────┬────────┘
         │
         │ Distribute to Subscribers
         ▼
┌─────────────────────────────────────────┐
│         Multiple Subscribers            │
├─────────────────────────────────────────┤
│ • NotificationSubscriber                │
│ • CacheInvalidationSubscriber           │
│ • AnalyticsSubscriber                   │
│ • IntegrationSubscriber                 │
│ • AuditLogSubscriber                    │
└─────────────────────────────────────────┘
```

## Use Cases

### Use Case 1: Sending Notifications (Email, SMS, Push)

**Scenario**: When a user registers, multiple notification actions need to happen:
- Send welcome email
- Send SMS verification code
- Send push notification

**Solution**: Publish `UserRegisteredEvent` once, multiple subscribers handle different notification types.

**Example Flow**:
1. UserController publishes `UserRegisteredEvent`
2. NotificationSubscriber sends welcome email
3. SMSSubscriber sends verification SMS
4. PushNotificationSubscriber sends mobile push

**Benefits**:
- If email fails, SMS can still be sent
- Easy to add new notification types
- No coupling between registration logic and notification logic

---

### Use Case 2: Cache Invalidation

**Scenario**: When data changes, you need to invalidate related caches.

**Solution**: Publish update events, cache invalidation subscriber clears relevant caches.

**Example Flow**:
1. UserController publishes `UserProfileUpdatedEvent`
2. CacheInvalidationSubscriber removes:
   - `user:{userId}` cache
   - `user:email:{email}` cache
   - Related caches

**Benefits**:
- Automatic cache invalidation
- No manual cache management in controllers
- Multiple cache layers can be invalidated independently

---

### Use Case 3: Analytics and Metrics

**Scenario**: Track user behavior and system metrics without impacting main application performance.

**Solution**: Analytics subscriber processes events asynchronously.

**Example Flow**:
1. BlogController publishes `BlogPostPublishedEvent`
2. AnalyticsSubscriber:
   - Tracks event in analytics system
   - Updates author statistics
   - Updates category statistics
   - Updates trending posts

**Benefits**:
- Non-blocking analytics
- Can handle high-volume events
- Easy to integrate with external analytics tools

---

### Use Case 4: Microservice Integration

**Scenario**: Your blog service needs to integrate with external services:
- CRM system
- Email marketing platform
- Social media platforms
- Search index

**Solution**: Integration subscriber handles all external API calls.

**Example Flow**:
1. BlogController publishes `BlogPostPublishedEvent`
2. IntegrationSubscriber:
   - Updates search index (Elasticsearch)
   - Posts to Twitter
   - Posts to LinkedIn
   - Updates email campaign

**Benefits**:
- External service failures don't affect main application
- Easy to add/remove integrations
- Can retry failed integrations automatically

---

### Use Case 5: Audit Logging

**Scenario**: Maintain compliance by logging all important events.

**Solution**: Audit subscriber logs all events to audit database.

**Example Flow**:
1. Any controller publishes an event
2. AuditLogSubscriber writes to audit log
3. Audit log maintained separately for compliance

**Benefits**:
- Centralized audit logging
- No performance impact on main operations
- Can meet compliance requirements (GDPR, SOC2, etc.)

---

### Use Case 6: Multi-Step Workflows

**Scenario**: Blog post publication requires multiple steps:
1. Notify author
2. Update search index
3. Share to social media
4. Update analytics
5. Invalidate cache

**Solution**: Single event triggers all steps in parallel.

**Example**:
```csharp
// In BlogController
await _eventBus.Publish(new BlogPostPublishedEvent { ... });

// This automatically triggers:
// ✅ NotificationSubscriber -> sends email to author
// ✅ IntegrationSubscriber -> updates search index
// ✅ IntegrationSubscriber -> shares to social media
// ✅ AnalyticsSubscriber -> tracks publication
// ✅ CacheInvalidationSubscriber -> clears caches
```

---

### Use Case 7: Event Sourcing Pattern

**Scenario**: Maintain a log of all state changes for replay/debugging.

**Solution**: Store all events, replay to reconstruct state.

**Example**: All events are stored in SQL Server by CAP. You can:
- Replay events to rebuild state
- Debug issues by examining event history
- Implement time-travel debugging

---

### Use Case 8: Saga Pattern (Distributed Transactions)

**Scenario**: Multi-step transaction across services (e.g., order processing).

**Solution**: Use CAP's outbox pattern for reliable distributed transactions.

**Example**:
```csharp
// Start transaction
using var transaction = await BeginTransactionAsync();

// Save order
await _orderRepository.SaveAsync(order, transaction);

// Publish event (stored in outbox, not published yet)
await _capPublisher.PublishAsync("OrderCreated", order, transaction);

// Commit transaction (only then is event published)
await transaction.CommitAsync();
```

---

## Implementation Examples

### Publishing Events from Controllers

```csharp
[HttpPost("register")]
public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
{
    // 1. Perform business logic
    var userId = Guid.NewGuid();
    // ... save user to database ...

    // 2. Publish event
    var @event = new UserRegisteredEvent
    {
        UserId = userId,
        Email = request.Email,
        Username = request.Username
    };
    
    await _eventBus.Publish(@event);
    
    return Ok(new { UserId = userId });
}
```

### Creating Subscribers

```csharp
public class NotificationSubscriber : ICapSubscribe
{
    private readonly IEmailService _emailService;
    
    [CapSubscribe("UserRegisteredEvent")]
    public async Task HandleUserRegistered(UserRegisteredEvent @event)
    {
        await _emailService.SendWelcomeEmail(@event.Email);
    }
}
```

### Registering Subscribers

In `Program.cs`:
```csharp
builder.Services.AddTransient<NotificationSubscriber>();
builder.Services.AddTransient<CacheInvalidationSubscriber>();
builder.Services.AddTransient<AnalyticsSubscriber>();
// ... register all subscribers
```

## Real-World Scenarios

### Scenario 1: E-Commerce Order Processing

```csharp
// 1. Publish OrderCreatedEvent
await _eventBus.Publish(new OrderCreatedEvent { OrderId = orderId, ... });

// Subscribers:
// - InventorySubscriber: Reserve inventory
// - PaymentSubscriber: Process payment
// - NotificationSubscriber: Send confirmation email
// - AnalyticsSubscriber: Track sales
// - AuditLogSubscriber: Log transaction
```

### Scenario 2: Content Management System

```csharp
// Publish ArticlePublishedEvent
await _eventBus.Publish(new ArticlePublishedEvent { ... });

// Subscribers:
// - SearchIndexSubscriber: Update Elasticsearch
// - CDNSubscriber: Invalidate CDN cache
// - RSSFeedSubscriber: Update RSS feed
// - SocialMediaSubscriber: Post to social platforms
// - AnalyticsSubscriber: Track content performance
```

### Scenario 3: User Management System

```csharp
// Publish UserDeactivatedEvent
await _eventBus.Publish(new UserDeactivatedEvent { ... });

// Subscribers:
// - CacheSubscriber: Clear user caches
// - SessionSubscriber: Invalidate active sessions
// - NotificationSubscriber: Send deactivation email
// - DataRetentionSubscriber: Schedule data deletion
// - AuditLogSubscriber: Log security event
```

## Best Practices

### 1. Event Naming
- Use past tense: `UserRegisteredEvent`, not `RegisterUserEvent`
- Be specific: `BlogPostPublishedEvent`, not `PostEvent`
- One event per action

### 2. Event Design
- Include all necessary data in the event
- Events should be immutable
- Keep events focused and small

### 3. Subscriber Design
- One subscriber per concern (separation of concerns)
- Make subscribers idempotent (safe to process multiple times)
- Handle errors gracefully

### 4. Error Handling
```csharp
[CapSubscribe("MyEvent")]
public async Task Handle(MyEvent @event)
{
    try
    {
        // Process event
    }
    catch (Exception ex)
    {
        // Log error - CAP will retry automatically
        _logger.LogError(ex, "Error processing event");
        throw; // Re-throw to trigger retry
    }
}
```

### 5. Performance
- Keep subscriber methods fast
- Use async/await properly
- Consider batching for high-volume events

### 6. Testing
- Test publishers and subscribers independently
- Use integration tests to verify event flow
- Mock external dependencies in subscribers

## Advanced Patterns

### Pattern 1: Event Chaining
```csharp
// Subscriber can publish new events
[CapSubscribe("UserRegisteredEvent")]
public async Task Handle(UserRegisteredEvent @event)
{
    // Process registration
    // ...
    
    // Publish follow-up event
    await _capPublisher.PublishAsync("UserOnboardingStarted", 
        new UserOnboardingStartedEvent { UserId = @event.UserId });
}
```

### Pattern 2: Conditional Processing
```csharp
[CapSubscribe("BlogPostCreatedEvent")]
public async Task Handle(BlogPostCreatedEvent @event)
{
    if (@event.IsPublished)
    {
        // Only process if published
        await ProcessPublishedPost(@event);
    }
}
```

### Pattern 3: Event Filtering
```csharp
[CapSubscribe("BlogPostPublishedEvent")]
public async Task Handle(BlogPostPublishedEvent @event)
{
    // Filter by category
    if (@event.Category == "Technology")
    {
        await ShareToTechNewsletter(@event);
    }
}
```

## Monitoring and Debugging

### CAP Dashboard (Optional)
Uncomment in `Program.cs`:
```csharp
options.UseDashboard();
```

Access at: `http://localhost:5000/cap` (default)

### Failed Messages
CAP automatically retries failed messages. Check:
- SQL Server `[cap].[Published]` table for published messages
- SQL Server `[cap].[Received]` table for received messages
- Failed messages are retried based on configuration

## Next Steps

1. Review example events in `UseCases/Events/`
2. Review example controllers in `UseCases/Controllers/`
3. Review example subscribers in `UseCases/Subscribers/`
4. Register your subscribers in `Program.cs`
5. Start publishing and consuming events!

## Summary

CAP library enables robust, scalable event-driven architecture. Key advantages:

✅ **Decoupling**: Services don't depend on each other  
✅ **Scalability**: Add subscribers without changing publishers  
✅ **Reliability**: Automatic retry and message persistence  
✅ **Flexibility**: Easy to add/remove event handlers  
✅ **Performance**: Asynchronous processing doesn't block main flow  

Use events for anything that doesn't need immediate response:
- Notifications
- Cache invalidation
- Analytics
- Integrations
- Audit logging
- Background processing


# CAP Library Use Cases - Examples and Documentation

This folder contains comprehensive examples demonstrating how to use the CAP library for event-driven messaging.

## 📁 Folder Structure

```
UseCases/
├── Events/                    # Example event classes
│   ├── UserRegisteredEvent.cs
│   ├── UserEmailVerifiedEvent.cs
│   ├── BlogPostCreatedEvent.cs
│   ├── BlogPostPublishedEvent.cs
│   ├── CommentAddedEvent.cs
│   └── UserProfileUpdatedEvent.cs
│
├── Controllers/               # Example API controllers that publish events
│   ├── UserController.cs
│   └── BlogController.cs
│
├── Subscribers/              # Example CAP subscribers that consume events
│   ├── NotificationSubscriber.cs       # Sends emails/notifications
│   ├── CacheInvalidationSubscriber.cs  # Manages cache invalidation
│   ├── AnalyticsSubscriber.cs          # Tracks analytics and metrics
│   ├── IntegrationSubscriber.cs        # Integrates with external services
│   └── AuditLogSubscriber.cs           # Maintains audit logs
│
├── README.md                 # This file
├── QUICK_START.md           # Quick start guide
└── USE_CASES.md             # Detailed use case documentation
```

## 🚀 Quick Links

- **[Quick Start Guide](QUICK_START.md)** - Get started in 5 minutes
- **[Use Cases Documentation](USE_CASES.md)** - Comprehensive guide with all use cases
- **[Example Controllers](Controllers/)** - See how to publish events from API endpoints
- **[Example Subscribers](Subscribers/)** - See how to consume events

## 📚 What's Included

### Events (6 Examples)
- **UserRegisteredEvent** - Published when a user registers
- **UserEmailVerifiedEvent** - Published when email is verified
- **UserProfileUpdatedEvent** - Published when profile is updated
- **BlogPostCreatedEvent** - Published when a blog post is created
- **BlogPostPublishedEvent** - Published when a blog post is published
- **CommentAddedEvent** - Published when a comment is added

### Controllers (2 Examples)
- **UserController** - Demonstrates publishing user-related events
- **BlogController** - Demonstrates publishing blog-related events

### Subscribers (5 Examples)
- **NotificationSubscriber** - Sends notifications (emails, SMS)
- **CacheInvalidationSubscriber** - Invalidates caches when data changes
- **AnalyticsSubscriber** - Tracks events for analytics
- **IntegrationSubscriber** - Integrates with external services (CRM, social media, search)
- **AuditLogSubscriber** - Maintains audit logs for compliance

## 🎯 Use Cases Covered

1. **✅ Notifications** - Send emails, SMS, push notifications asynchronously
2. **✅ Cache Management** - Automatic cache invalidation on data changes
3. **✅ Analytics** - Track user behavior and system metrics
4. **✅ Microservice Integration** - Integrate with external services
5. **✅ Audit Logging** - Maintain compliance audit trails
6. **✅ Multi-Step Workflows** - Orchestrate complex processes
7. **✅ Event Sourcing** - Maintain event log for state reconstruction
8. **✅ Saga Pattern** - Distributed transactions across services

## 🏃 Getting Started

### 1. Review the Quick Start Guide
Start with [QUICK_START.md](QUICK_START.md) to understand the basics.

### 2. Explore Example Events
Check out the event classes in [Events/](Events/) to see how events are structured.

### 3. See Controllers in Action
Review [Controllers/](Controllers/) to see how events are published from API endpoints.

### 4. Understand Subscribers
Look at [Subscribers/](Subscribers/) to see how events are consumed.

### 5. Read Detailed Documentation
Read [USE_CASES.md](USE_CASES.md) for comprehensive examples and best practices.

## 🔧 How to Use These Examples

### Running the Examples

1. **Ensure RabbitMQ is running**:
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

2. **Ensure SQL Server is accessible** (configured in appsettings.json)

3. **Subscribers are already registered** in `Program.cs`:
   ```csharp
   builder.Services.AddTransient<NotificationSubscriber>();
   builder.Services.AddTransient<CacheInvalidationSubscriber>();
   // ... etc
   ```

4. **Test the endpoints**:
   - `POST /api/user/register` - Publishes `UserRegisteredEvent`
   - `POST /api/blog/posts` - Publishes `BlogPostCreatedEvent`
   - `POST /api/blog/posts/{id}/publish` - Publishes `BlogPostPublishedEvent`
   - And more...

### Example API Call

```bash
# Register a user - this will trigger multiple subscribers
POST /api/user/register
Content-Type: application/json

{
  "email": "user@example.com",
  "username": "johndoe",
  "fullName": "John Doe",
  "password": "SecurePassword123!"
}
```

**What happens:**
1. User is registered in the database
2. `UserRegisteredEvent` is published
3. Multiple subscribers process the event:
   - ✅ NotificationSubscriber sends welcome email
   - ✅ AnalyticsSubscriber tracks registration
   - ✅ IntegrationSubscriber syncs to CRM
   - ✅ AuditLogSubscriber logs the event

## 📖 Key Concepts

### Event-Driven Architecture
- **Publisher** doesn't know about subscribers
- **Subscribers** process events independently
- **Loose coupling** between components
- **Scalable** - easy to add new subscribers

### CAP Benefits
- ✅ **Reliable** - Messages persisted in SQL Server
- ✅ **Retry** - Automatic retry on failure
- ✅ **Ordered** - Maintains message order
- ✅ **Transactional** - Outbox pattern support

## 🎓 Learning Path

1. **Start Here**: [QUICK_START.md](QUICK_START.md)
   - Basic concepts
   - Simple examples
   - How to publish/consume

2. **Then Read**: [USE_CASES.md](USE_CASES.md)
   - Detailed use cases
   - Real-world scenarios
   - Best practices

3. **Explore Code**: Review example files
   - See how events are structured
   - Understand controller patterns
   - Learn subscriber patterns

4. **Build Your Own**: Create custom events and subscribers for your use case

## 💡 Tips

- **Event Naming**: Use past tense (UserRegisteredEvent, not RegisterUserEvent)
- **Idempotency**: Make subscribers safe to run multiple times
- **Error Handling**: Let CAP handle retries, log errors appropriately
- **Testing**: Test publishers and subscribers independently
- **Monitoring**: Use CAP Dashboard to monitor message flow

## 📝 Next Steps

1. ✅ Review all example files
2. ✅ Try calling the example endpoints
3. ✅ Check logs to see subscribers in action
4. ✅ Create your own events and subscribers
5. ✅ Integrate CAP into your application

## 🔗 Related Documentation

- [CAP Library Official Documentation](https://cap.dotnetcore.xyz/)
- [CAP Bus README](../CAPBus/README.md) - Configuration guide
- [Main Project README](../README.md) - Project overview

---

**Happy Event-Driven Coding! 🚀**


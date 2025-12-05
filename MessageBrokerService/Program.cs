using DotNetCore.CAP;
using MessageBrokerService.CAPBus;
using MessageBrokerService.IBus;
using MessageBrokerService.Services;
using MessageBrokerService.UseCases.Subscribers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configure CAP with RabbitMQ and SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var rabbitMQHostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost";
var rabbitMQUserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest";
var rabbitMQPassword = builder.Configuration["RabbitMQ:Password"] ?? "guest";
var rabbitMQPort = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672");

builder.Services.AddCap(options =>
{
    // Use SQL Server as storage
    options.UseSqlServer(opt =>
    {
        opt.ConnectionString = connectionString;
        opt.Schema = "cap"; // Optional: specify schema name
    });

    // Use RabbitMQ as message transport
    options.UseRabbitMQ(opt =>
    {
        opt.HostName = rabbitMQHostName;
        opt.UserName = rabbitMQUserName;
        opt.Password = rabbitMQPassword;
        opt.Port = rabbitMQPort;
        opt.VirtualHost = "/";
    });

    // Optional: Configure CAP dashboard (useful for monitoring)
    // options.UseDashboard();
    
    // Configure failed message retry
    options.FailedRetryCount = 5;
    options.FailedRetryInterval = 60;
});

// Register CAP Event Bus
builder.Services.AddTransient<IEventBus, CAPEventBus>();

// Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Register CAP Subscribers (Examples - Uncomment as needed)
// These subscribers demonstrate different use cases:
builder.Services.AddTransient<NotificationSubscriber>();      // Use Case 1: Notifications
builder.Services.AddTransient<CacheInvalidationSubscriber>(); // Use Case 2: Cache Management
builder.Services.AddTransient<AnalyticsSubscriber>();         // Use Case 3: Analytics
builder.Services.AddTransient<IntegrationSubscriber>();       // Use Case 4: External Integrations
builder.Services.AddTransient<AuditLogSubscriber>();          // Use Case 5: Audit Logging

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable static files FIRST (before other middleware)
// UseDefaultFiles must come before UseStaticFiles
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseStaticFiles();

app.UseHttpsRedirection();

// Map API controllers - these should be matched before fallback
app.MapControllers();

// Map fallback route to index.html for SPA behavior
// This must be LAST so API routes are checked first
app.MapFallbackToFile("index.html");

app.Run();

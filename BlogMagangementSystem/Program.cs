using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlogMagangementSystem.Common.Helpers;

namespace BlogMagangementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddServices(builder.Configuration);
            
            // Configure Autofac as the service provider
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule<AutofacModule>();
            });
            builder. Logging.AddConsole();
            builder. Logging.AddDebug();
            builder.Logging.ClearProviders();

           Log.Logger = new LoggerConfiguration().WriteTo.Seq("http://localhost:5341")
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.MSSqlServer(
              connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
              restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
              sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Logs" }
              ).CreateLogger();
            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<UpdatePostDto>
            (
                "Your post is up todate",
                job => job.Run(),  // method to call
                Cron.Minutely       // every minute
            );

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<GlobalTransactionMiddleware>();   
            app.UseHangfireDashboard("/hangfire");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

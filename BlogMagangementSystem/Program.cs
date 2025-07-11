using BlogMagangementSystem.Common.Context;
using BlogMagangementSystem.Common.ExtensionMethods;
using BlogMagangementSystem.Common.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BlogMagangementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddServices(builder.Configuration);
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<SerilogMiddleware>();
            app.UseMiddleware<GlobalTransactionMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

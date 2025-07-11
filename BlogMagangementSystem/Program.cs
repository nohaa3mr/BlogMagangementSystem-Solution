using BlogMagangementSystem.Common.Context;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
           builder. Logging.AddConsole();
           builder. Logging.AddDebug();
            builder.Logging.ClearProviders();

            Serilog.Log.Logger = new LoggerConfiguration().WriteTo.Seq("http://localhost:5341")
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

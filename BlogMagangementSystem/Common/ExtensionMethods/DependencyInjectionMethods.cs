using BlogMagangementSystem.Common.Context;
using BlogMagangementSystem.Common.ErrorHandling;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Middlewares;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.AddPostFeature.Validators.PostValidation;
using BlogMagangementSystem.Features.DeletePostFeature;
using BlogMagangementSystem.Features.GetPostByIdFeature;
using BlogMagangementSystem.Features.PostModule;
using BlogMagangementSystem.Features.UpdatePostFeature;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Configuration;


namespace BlogMagangementSystem.Common.ExtensionMethods
{
    public static class DependencyInjectionMethods
    {
        public static IServiceCollection AddServices(this IServiceCollection Services, IConfiguration Configuration)
        {
          Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
          Services.AddEndpointsApiExplorer();
          Services.AddSwaggerGen();
          Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            Services.AddScoped(typeof(GenericRepository<>));
            Services.AddScoped<IValidator<AddPostRequestViewModel>, AddPostRequestVMValidator>();
            Services.AddScoped<IValidator<UpdatePostRequestViewModel>, UpdatePostRequestVMValidator>();
            Services.AddScoped<IValidator<GetPostByIdRequestViewModel>, GetPostByIdValidator>();
            Services.AddScoped<IValidator<DeletePostRequestViewModel>, DeletePostRequesVMValidator>();
            Services.AddMediatR(cfg =>
              cfg.RegisterServicesFromAssemblies(
                  typeof(Program).Assembly
              )
          );
            Services.AddScoped<ExceptionMiddleware>();
            Services.AddScoped<SerilogMiddleware>();
            Services.AddScoped<GlobalTransactionMiddleware>();
            Services.AddScoped<BaseRequestParameters>();
            Services.AddScoped(typeof(BaseEndpointParameters<>));
            Services.AddHangfire(opt =>
            {
                opt.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
                opt.UseRecommendedSerializerSettings();
                opt.UseSimpleAssemblyNameTypeSerializer();
                opt.UseRecommendedSerializerSettings();
            }); 
            Services.AddHangfireServer();
            Services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory { HostName = "localhost" }; // Adjust settings as needed
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            Services.AddSingleton<IChannel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateChannelAsync().GetAwaiter().GetResult();
            });
            Services.AddHttpContextAccessor();
            Services.AddCap(options =>
            {
                options.UseEntityFramework<BlogDbContext>();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseRabbitMQ(rabbitMQ =>
                {
                    rabbitMQ.HostName = "localhost";
                    rabbitMQ.UserName = "guest";
                    rabbitMQ.Password = "guest";
                    rabbitMQ.Port = 15672;
                    rabbitMQ.ExchangeName = "cap.default.router";

                });
            });
            #region ApiValidationError
            Services.Configure<ApiBehaviorOptions>(opthion =>
            {
                opthion.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(o => o.Value?.Errors.Count() > 0)
                                                         .SelectMany(o => o.Value.Errors)
                                                         .Select(e => e.ErrorMessage)
                                                         .ToList();
                    var response = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            #endregion

            return Services;

        }
    }
}

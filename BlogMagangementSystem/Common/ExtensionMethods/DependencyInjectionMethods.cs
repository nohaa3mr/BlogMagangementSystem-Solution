using BlogMagangementSystem.Common.Context;
using BlogMagangementSystem.Common.ErrorHandling;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.JWT_Service;
using BlogMagangementSystem.Common.Middlewares;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Common.Validators.PostValidation;
using BlogMagangementSystem.Features.PostFeatures.AddPostFeature;
using BlogMagangementSystem.Features.PostFeatures.DeletePostFeature;
using BlogMagangementSystem.Features.PostFeatures.GetPostByIdFeature;
using BlogMagangementSystem.Features.PostFeatures.UpdatePostFeature;
using BlogMagangementSystem.Features.UserFeatures.HashingAlgorithm;
using BlogMagangementSystem.Features.UserFeatures.Login;
using BlogMagangementSystem.Features.UserFeatures.Registeration;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
            Services.AddScoped<JWTService>();
            Services.AddScoped<UserNameHaser>();
            Services.AddScoped<PasswordHasher>();
            Services.AddScoped<IValidator<AddPostRequestViewModel>, AddPostRequestVMValidator>();
            Services.AddScoped<IValidator<UpdatePostRequestViewModel>, UpdatePostRequestVMValidator>();
            Services.AddScoped<IValidator<GetPostByIdRequestViewModel>, GetPostByIdValidator>();
            Services.AddScoped<IValidator<DeletePostRequestViewModel>, DeletePostRequesVMValidator>();
            Services.AddScoped<IValidator<UserRegisterationRequestViewModel>,UserRequestVmValidator>();
            Services.AddScoped<IValidator<UserLoginRequestViewModel>, LoginRequestViewModelValidator>();   
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
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
             options.Authority= "https://your-auth-server.com"; // OAuth2 provider (e.g. Auth0, Azure AD, IdentityServer)
             options.Audience = "http://localhost:7185/"; 
             options.TokenValidationParameters = new TokenValidationParameters
             {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
            };
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

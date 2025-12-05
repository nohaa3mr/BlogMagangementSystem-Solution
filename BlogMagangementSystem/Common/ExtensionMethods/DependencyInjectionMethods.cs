using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlogMagangementSystem.Common.Helpers;
using System.Text;

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
            Services.AddScoped<UserNameHasher>();
            Services.AddScoped<PasswordHasher>();
            
            // Note: All validators are now automatically registered via AutofacModule
            // No need to manually register each validator here

            Services.AddMediatR(cfg =>
              cfg.RegisterServicesFromAssemblies(
                  typeof(Program).Assembly
              )
          );
            Services.AddScoped<ExceptionMiddleware>();
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
          Services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
       //   Services.AddTransient<EmailService.EmailService>();

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
            var jwtKey = Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
            {
                throw new ArgumentException("JWT Key must be at least 32 characters long");
            }

            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["Jwt:Issuer"],
                     ValidAudience = Configuration["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                 };
             });

            // Add Authorization Policies
            Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
                options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
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

using BlogMagangementSystem.Common.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

            return Services;

        }
    }
}

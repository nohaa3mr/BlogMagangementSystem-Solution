using Microsoft.EntityFrameworkCore.Design;

namespace BlogMagangementSystem.Common.Context
{
    public class BlogDbContextDesignFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        private const string ConnectionString = "Server=.;Database=BlogSystem;Trusted_Connection =true;TrustServerCertificate=true;MultipleActiveResultSets=true";

        public BlogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new BlogDbContext(optionsBuilder.Options);
        }
    }
}

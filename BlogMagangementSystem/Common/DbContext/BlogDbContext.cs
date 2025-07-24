
namespace BlogMagangementSystem.Common.Context
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(p => p.Status)
                .HasConversion<string>(); // 👈 This stores the enum as a string
            modelBuilder.Entity<User>().Property(u => u.Role)
                .HasConversion<string>(); // 👈 This stores the enum as a string
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
       
    }
}

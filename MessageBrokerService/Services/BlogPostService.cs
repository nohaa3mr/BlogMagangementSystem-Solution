using MessageBrokerService.Models;

namespace MessageBrokerService.Services
{
    public class BlogPostService : IBlogPostService
    {
        private static readonly List<BlogPost> _posts = new();
        private readonly ILogger<BlogPostService> _logger;

        public BlogPostService(ILogger<BlogPostService> logger)
        {
            _logger = logger;
        }

        public Task<List<BlogPost>> GetAllAsync()
        {
            return Task.FromResult(_posts.OrderByDescending(p => p.CreatedAt).ToList());
        }

        public Task<List<BlogPost>> GetByAuthorAsync(Guid authorId)
        {
            return Task.FromResult(_posts.Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedAt).ToList());
        }

        public Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_posts.FirstOrDefault(p => p.Id == id));
        }

        public Task<BlogPost?> GetBySlugAsync(string slug)
        {
            return Task.FromResult(_posts.FirstOrDefault(p => p.Slug == slug));
        }

        public Task<BlogPost> CreateAsync(BlogPost post)
        {
            post.Id = Guid.NewGuid();
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            if (post.IsPublished)
            {
                post.PublishedAt = DateTime.UtcNow;
            }
            _posts.Add(post);
            _logger.LogInformation("Blog post created: {PostId}", post.Id);
            return Task.FromResult(post);
        }

        public Task<BlogPost?> UpdateAsync(Guid id, BlogPost post)
        {
            var existing = _posts.FirstOrDefault(p => p.Id == id);
            if (existing == null) return Task.FromResult<BlogPost?>(null);

            existing.Title = post.Title;
            existing.Slug = post.Slug;
            existing.Content = post.Content;
            existing.Category = post.Category;
            existing.Excerpt = post.Excerpt;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Blog post updated: {PostId}", id);
            return Task.FromResult<BlogPost?>(existing);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return Task.FromResult(false);

            _posts.Remove(post);
            _logger.LogInformation("Blog post deleted: {PostId}", id);
            return Task.FromResult(true);
        }

        public Task<bool> PublishAsync(Guid id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return Task.FromResult(false);

            post.IsPublished = true;
            post.PublishedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            _logger.LogInformation("Blog post published: {PostId}", id);
            return Task.FromResult(true);
        }
    }
}


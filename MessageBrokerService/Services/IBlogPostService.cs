using MessageBrokerService.Models;

namespace MessageBrokerService.Services
{
    public interface IBlogPostService
    {
        Task<List<BlogPost>> GetAllAsync();
        Task<List<BlogPost>> GetByAuthorAsync(Guid authorId);
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> GetBySlugAsync(string slug);
        Task<BlogPost> CreateAsync(BlogPost post);
        Task<BlogPost?> UpdateAsync(Guid id, BlogPost post);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> PublishAsync(Guid id);
    }
}


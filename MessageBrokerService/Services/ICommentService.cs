using MessageBrokerService.Models;

namespace MessageBrokerService.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetByPostIdAsync(Guid postId);
        Task<Comment?> GetByIdAsync(Guid id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(Guid id, Comment comment);
        Task<bool> DeleteAsync(Guid id);
    }
}


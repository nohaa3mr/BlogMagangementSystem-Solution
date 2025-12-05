using MessageBrokerService.Models;

namespace MessageBrokerService.Services
{
    public class CommentService : ICommentService
    {
        private static readonly List<Comment> _comments = new();
        private readonly ILogger<CommentService> _logger;

        public CommentService(ILogger<CommentService> logger)
        {
            _logger = logger;
        }

        public Task<List<Comment>> GetAllAsync()
        {
            return Task.FromResult(_comments.OrderByDescending(c => c.CreatedAt).ToList());
        }

        public Task<List<Comment>> GetByPostIdAsync(Guid postId)
        {
            return Task.FromResult(_comments.Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt).ToList());
        }

        public Task<Comment?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_comments.FirstOrDefault(c => c.Id == id));
        }

        public Task<Comment> CreateAsync(Comment comment)
        {
            comment.Id = Guid.NewGuid();
            comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = DateTime.UtcNow;
            _comments.Add(comment);
            _logger.LogInformation("Comment created: {CommentId}", comment.Id);
            return Task.FromResult(comment);
        }

        public Task<Comment?> UpdateAsync(Guid id, Comment comment)
        {
            var existing = _comments.FirstOrDefault(c => c.Id == id);
            if (existing == null) return Task.FromResult<Comment?>(null);

            existing.Content = comment.Content;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Comment updated: {CommentId}", id);
            return Task.FromResult<Comment?>(existing);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            if (comment == null) return Task.FromResult(false);

            _comments.Remove(comment);
            _logger.LogInformation("Comment deleted: {CommentId}", id);
            return Task.FromResult(true);
        }
    }
}


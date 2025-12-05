using DotNetCore.CAP;
using MessageBrokerService.UseCases.Events;

namespace MessageBrokerService.UseCases.Subscribers
{
    /// <summary>
    /// Use Case 2: Cache Invalidation
    /// Automatically invalidate caches when data changes
    /// </summary>
    public class CacheInvalidationSubscriber : ICapSubscribe
    {
        private readonly ILogger<CacheInvalidationSubscriber> _logger;

        public CacheInvalidationSubscriber(ILogger<CacheInvalidationSubscriber> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Invalidate user cache when profile is updated
        /// </summary>
        [CapSubscribe("UserProfileUpdatedEvent")]
        public async Task HandleProfileUpdated(UserProfileUpdatedEvent @event)
        {
            _logger.LogInformation("Invalidating cache for user: {UserId}", @event.UserId);
            
            // In real application:
            // await _cacheService.RemoveAsync($"user:{@event.UserId}");
            // await _cacheService.RemoveAsync($"user:email:{@event.Email}");
            // if (@event.PreviousEmail != null)
            // {
            //     await _cacheService.RemoveAsync($"user:email:{@event.PreviousEmail}");
            // }
            
            await Task.Delay(50);
            _logger.LogInformation("Cache invalidated for user: {UserId}", @event.UserId);
        }

        /// <summary>
        /// Invalidate blog post cache when post is published
        /// </summary>
        [CapSubscribe("BlogPostPublishedEvent")]
        public async Task HandlePostPublished(BlogPostPublishedEvent @event)
        {
            _logger.LogInformation("Invalidating cache for post: {PostId}", @event.PostId);
            
            // In real application:
            // await _cacheService.RemoveAsync($"post:{@event.PostId}");
            // await _cacheService.RemoveAsync($"post:slug:{@event.Slug}");
            // await _cacheService.RemoveAsync("posts:recent"); // Invalidate list cache
            // await _cacheService.RemoveAsync($"posts:category:{category}"); // If category exists
            
            await Task.Delay(50);
            _logger.LogInformation("Cache invalidated for post: {PostId}", @event.PostId);
        }

        /// <summary>
        /// Invalidate post comments cache when new comment is added
        /// </summary>
        [CapSubscribe("CommentAddedEvent")]
        public async Task HandleCommentAdded(CommentAddedEvent @event)
        {
            _logger.LogInformation("Invalidating comments cache for post: {PostId}", @event.PostId);
            
            // In real application:
            // await _cacheService.RemoveAsync($"post:{@event.PostId}:comments");
            // await _cacheService.RemoveAsync($"post:{@event.PostId}:comments:count");
            
            await Task.Delay(50);
            _logger.LogInformation("Comments cache invalidated for post: {PostId}", @event.PostId);
        }
    }
}


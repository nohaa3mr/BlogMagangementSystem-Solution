using MessageBrokerService.IBus;
using MessageBrokerService.UseCases.Events;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerService.UseCases.Controllers
{
    /// <summary>
    /// Example Controller: Publishing events for blog-related operations
    /// Use Case 2: Publishing events at different stages of a workflow
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IEventBus eventBus, ILogger<BlogController> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        /// <summary>
        /// Create a new blog post and publish BlogPostCreatedEvent
        /// This event is published immediately after creation (draft or published)
        /// </summary>
        [HttpPost("posts")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var postId = Guid.NewGuid();
            var authorId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // In real app, get from auth context

            // 1. Save blog post to database
            _logger.LogInformation("Blog post created: {PostId}", postId);

            // 2. Publish creation event
            var createdEvent = new BlogPostCreatedEvent
            {
                PostId = postId,
                AuthorId = authorId,
                Title = request.Title,
                Slug = request.Slug,
                Category = request.Category,
                IsPublished = request.IsPublished
            };

            await _eventBus.Publish(createdEvent);

            // 3. If post is published, also publish published event
            if (request.IsPublished)
            {
                var publishedEvent = new BlogPostPublishedEvent
                {
                    PostId = postId,
                    AuthorId = authorId,
                    Title = request.Title,
                    Slug = request.Slug,
                    AuthorEmail = "author@example.com" // Fetch from database
                };

                await _eventBus.Publish(publishedEvent);
                _logger.LogInformation("Blog post published: {PostId}", postId);
            }

            return Ok(new { PostId = postId, Message = "Post created successfully" });
        }

        /// <summary>
        /// Publish an existing blog post and publish BlogPostPublishedEvent
        /// Use Case: Publishing a draft post later
        /// </summary>
        [HttpPost("posts/{postId}/publish")]
        public async Task<IActionResult> PublishPost(Guid postId)
        {
            // 1. Update post status in database
            _logger.LogInformation("Publishing blog post: {PostId}", postId);

            // 2. Publish event to notify all interested systems
            var @event = new BlogPostPublishedEvent
            {
                PostId = postId,
                AuthorId = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                Title = "Sample Blog Post", // In real app, fetch from database
                Slug = "sample-blog-post",
                AuthorEmail = "author@example.com"
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published BlogPostPublishedEvent for post: {PostId}", postId);

            return Ok(new { Message = "Post published successfully" });
        }

        /// <summary>
        /// Add a comment to a blog post and publish CommentAddedEvent
        /// </summary>
        [HttpPost("posts/{postId}/comments")]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] AddCommentRequest request)
        {
            var commentId = Guid.NewGuid();

            // 1. Save comment to database
            _logger.LogInformation("Comment added: {CommentId} to post: {PostId}", commentId, postId);

            // 2. Publish event
            var @event = new CommentAddedEvent
            {
                CommentId = commentId,
                PostId = postId,
                AuthorId = request.AuthorId,
                AuthorName = request.AuthorName,
                Content = request.Content,
                ParentCommentId = request.ParentCommentId
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published CommentAddedEvent for comment: {CommentId}", commentId);

            return Ok(new { CommentId = commentId, Message = "Comment added successfully" });
        }
    }

    // Request DTOs
    public class CreatePostRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }

    public class AddCommentRequest
    {
        public Guid? AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid? ParentCommentId { get; set; }
    }
}


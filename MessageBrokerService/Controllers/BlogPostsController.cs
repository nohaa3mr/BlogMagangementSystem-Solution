using MessageBrokerService.IBus;
using MessageBrokerService.Models;
using MessageBrokerService.Services;
using MessageBrokerService.UseCases.Events;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(
            IBlogPostService blogPostService,
            IUserService userService,
            IEventBus eventBus,
            ILogger<BlogPostsController> logger)
        {
            _blogPostService = blogPostService;
            _userService = userService;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogPost>>> GetAll()
        {
            var posts = await _blogPostService.GetAllAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetById(Guid id)
        {
            var post = await _blogPostService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> Create([FromBody] BlogPost post)
        {
            var createdPost = await _blogPostService.CreateAsync(post);

            // Always publish BlogPostCreatedEvent
            var createdEvent = new BlogPostCreatedEvent
            {
                PostId = createdPost.Id,
                AuthorId = createdPost.AuthorId,
                Title = createdPost.Title,
                Slug = createdPost.Slug,
                Category = createdPost.Category,
                IsPublished = createdPost.IsPublished
            };

            await _eventBus.Publish(createdEvent);
            _logger.LogInformation("Published BlogPostCreatedEvent for post: {PostId}", createdPost.Id);

            // If published, also publish BlogPostPublishedEvent
            if (createdPost.IsPublished)
            {
                var author = await _userService.GetByIdAsync(createdPost.AuthorId);
                var publishedEvent = new BlogPostPublishedEvent
                {
                    PostId = createdPost.Id,
                    AuthorId = createdPost.AuthorId,
                    Title = createdPost.Title,
                    Slug = createdPost.Slug,
                    AuthorEmail = author?.Email ?? string.Empty
                };

                await _eventBus.Publish(publishedEvent);
                _logger.LogInformation("Published BlogPostPublishedEvent for post: {PostId}", createdPost.Id);
            }

            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BlogPost>> Update(Guid id, [FromBody] BlogPost post)
        {
            var updatedPost = await _blogPostService.UpdateAsync(id, post);
            if (updatedPost == null) return NotFound();

            // If status changed to published, publish BlogPostPublishedEvent
            var existingPost = await _blogPostService.GetByIdAsync(id);
            if (post.IsPublished && (!existingPost?.IsPublished ?? true))
            {
                var author = await _userService.GetByIdAsync(updatedPost.AuthorId);
                var publishedEvent = new BlogPostPublishedEvent
                {
                    PostId = updatedPost.Id,
                    AuthorId = updatedPost.AuthorId,
                    Title = updatedPost.Title,
                    Slug = updatedPost.Slug,
                    AuthorEmail = author?.Email ?? string.Empty
                };

                await _eventBus.Publish(publishedEvent);
                _logger.LogInformation("Published BlogPostPublishedEvent for post: {PostId}", updatedPost.Id);
            }

            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _blogPostService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var post = await _blogPostService.GetByIdAsync(id);
            if (post == null) return NotFound();

            var result = await _blogPostService.PublishAsync(id);
            if (!result) return BadRequest("Failed to publish post");

            // Publish BlogPostPublishedEvent
            var author = await _userService.GetByIdAsync(post.AuthorId);
            var @event = new BlogPostPublishedEvent
            {
                PostId = post.Id,
                AuthorId = post.AuthorId,
                Title = post.Title,
                Slug = post.Slug,
                AuthorEmail = author?.Email ?? string.Empty
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published BlogPostPublishedEvent for post: {PostId}", post.Id);

            return Ok(new { message = "Post published and event published" });
        }
    }
}


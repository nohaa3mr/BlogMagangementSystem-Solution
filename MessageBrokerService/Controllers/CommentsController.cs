using MessageBrokerService.IBus;
using MessageBrokerService.Models;
using MessageBrokerService.Services;
using MessageBrokerService.UseCases.Events;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(
            ICommentService commentService,
            IEventBus eventBus,
            ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAll([FromQuery] Guid? postId)
        {
            List<Comment> comments;
            if (postId.HasValue)
            {
                comments = await _commentService.GetByPostIdAsync(postId.Value);
            }
            else
            {
                comments = await _commentService.GetAllAsync();
            }
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetById(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> Create([FromBody] Comment comment)
        {
            var createdComment = await _commentService.CreateAsync(comment);

            // Publish CommentAddedEvent
            var @event = new CommentAddedEvent
            {
                CommentId = createdComment.Id,
                PostId = createdComment.PostId,
                AuthorId = createdComment.AuthorId,
                AuthorName = createdComment.AuthorName,
                Content = createdComment.Content,
                ParentCommentId = createdComment.ParentCommentId
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published CommentAddedEvent for comment: {CommentId}", createdComment.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdComment.Id }, createdComment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Comment>> Update(Guid id, [FromBody] Comment comment)
        {
            var updatedComment = await _commentService.UpdateAsync(id, comment);
            if (updatedComment == null) return NotFound();

            return Ok(updatedComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _commentService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}


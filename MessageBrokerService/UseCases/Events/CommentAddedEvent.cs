using MessageBrokerService.Events;

namespace MessageBrokerService.UseCases.Events
{
    /// <summary>
    /// Event: Published when a comment is added to a blog post
    /// </summary>
    public class CommentAddedEvent : Event
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public Guid? AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid? ParentCommentId { get; set; }
    }
}


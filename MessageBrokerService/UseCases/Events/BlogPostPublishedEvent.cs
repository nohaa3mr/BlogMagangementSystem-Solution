using MessageBrokerService.Events;

namespace MessageBrokerService.UseCases.Events
{
    /// <summary>
    /// Event: Published when a blog post is published (made public)
    /// </summary>
    public class BlogPostPublishedEvent : Event
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string AuthorEmail { get; set; } = string.Empty;
    }
}


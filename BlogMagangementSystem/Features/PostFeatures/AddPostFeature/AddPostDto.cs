namespace BlogMagangementSystem.Features.PostFeatures.AddPostFeature
{
    public class AddPostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = false;
        public string Username { get; set; }
        public bool IsSendToMessageBroker { get;  set; }
        public string Message { get; set; }
    }
}

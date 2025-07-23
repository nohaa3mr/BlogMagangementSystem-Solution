namespace BlogMagangementSystem.Features.PostFeatures.GetPostByIdFeature
{
    public class GetPostByIdResponseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Username { get; set; }

    }
}

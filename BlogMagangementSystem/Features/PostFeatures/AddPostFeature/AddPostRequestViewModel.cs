namespace BlogMagangementSystem.Features.PostFeatures.AddPostFeature
{
    public class AddPostRequestViewModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } 
        



    }
}

namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public class UpdatePostRequestViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

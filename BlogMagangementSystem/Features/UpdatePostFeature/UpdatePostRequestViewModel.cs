namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public class UpdatePostRequestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<string> Comments { get; set; } = new HashSet<string>();
        public ICollection<string> Tags { get; set; } = new HashSet<string>();
    }
}

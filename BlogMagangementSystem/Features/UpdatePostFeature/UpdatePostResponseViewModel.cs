namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public class UpdatePostResponseViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Username { get; set; }
        public ICollection<string> Comments { get; set; } = new HashSet<string>();
        public ICollection<string> Tags { get; set; } = new HashSet<string>();


    }
}

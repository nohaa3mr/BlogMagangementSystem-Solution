using BlogMagangementSystem.Common.Entities;

namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public class UpdatePostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<string> Comments { get; set; } = new HashSet<string>();
        public ICollection<string> Tags { get; set; } = new HashSet<string>();

    }
}
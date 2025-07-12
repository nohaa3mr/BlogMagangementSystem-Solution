using BlogMagangementSystem.Common.Enums;

namespace BlogMagangementSystem.Common.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public PostStatus Status { get; set; } = PostStatus.Draft; // e.g., Draft, Published, Archived

    }
}

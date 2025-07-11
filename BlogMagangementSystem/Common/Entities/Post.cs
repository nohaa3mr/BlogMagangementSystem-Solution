namespace BlogMagangementSystem.Common.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public int CategoryId { get; set; } 
        public Category Category { get; set; } 

    }
}

namespace BlogMagangementSystem.Common.Entities
{
    public class Comment :BaseEntity
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }

}

namespace BlogMagangementSystem.Common.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; }
    public ICollection<User> Users { get; set; } = new HashSet<User>();
    public int UserId { get; set; }
    public DateTime SentAt { get; set; }



}


namespace BlogMagangementSystem.Common.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int PhoneNumber { get; set; } 
    public string Address { get; set; }
    public Role Role { get; set; }        // e.g., Admin, User, Moderator
    public string Username { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
}

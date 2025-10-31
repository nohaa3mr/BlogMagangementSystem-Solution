namespace BlogMagangementSystem.Common.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Permissions { get; set; } 
    public ICollection<User> Users { get; set; } = new List<User>();
     

}
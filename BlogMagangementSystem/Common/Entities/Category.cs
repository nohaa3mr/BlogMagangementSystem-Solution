namespace BlogMagangementSystem.Common.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Post> posts { get; set; } = new HashSet<Post>();

}

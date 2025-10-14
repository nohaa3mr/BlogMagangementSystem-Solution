namespace BlogMagangementSystem.Common.Entities
{
    public class BaseEntity 
    {
        public Guid ID { get; set; } = new Guid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public  bool IsDeleted { get ; set; } = false;
    }
}

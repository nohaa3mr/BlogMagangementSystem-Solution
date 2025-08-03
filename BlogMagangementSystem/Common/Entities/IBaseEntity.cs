namespace BlogMagangementSystem.Common.Entities
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}

namespace BlogMagangementSystem.Features.Comments.GetCommentById
{
    public class GetCommentByIdResponseViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}


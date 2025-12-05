namespace BlogMagangementSystem.Features.Comments.CreateComment.DTOs
{
    public class CreateCommentResponseDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

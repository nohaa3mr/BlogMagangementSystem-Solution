namespace BlogMagangementSystem.Features.Comments.CreateComment.DTOs
{
    public class CreateCommentRequestDTO
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}

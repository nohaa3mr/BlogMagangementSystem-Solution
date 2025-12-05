namespace BlogMagangementSystem.Features.Comments.CreateComment.ViewModels
{
    public class CreateCommentRequestViewModel
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }
}

using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.Comments.DeleteComment;

namespace BlogMagangementSystem.Features.Comments.DeleteComment
{
    public sealed record DeleteCommentCommand(Guid Id, Guid UserId) : IRequest<RequestResult<DeleteCommentResponseViewModel>>;

    public class DeleteCommentCommandHandler : BaseRequestHandler<DeleteCommentCommand, RequestResult<DeleteCommentResponseViewModel>>
    {
        private readonly GenericRepository<Comment> _commentRepository;

        public DeleteCommentCommandHandler(
            BaseRequestParameters parameters,
            GenericRepository<Comment> commentRepository) : base(parameters)
        {
            _commentRepository = commentRepository;
        }

        public override async Task<RequestResult<DeleteCommentResponseViewModel>> Handle(
            DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);
            
            if (comment is null || comment.IsDeleted)
                return RequestResult<DeleteCommentResponseViewModel>. Failure(ErrorCode.DoesNotExist);

            // Check if user owns the comment or is admin
            if (comment.UserId != request.UserId)
            {
                return RequestResult<DeleteCommentResponseViewModel>.Failure(ErrorCode.NotAuthorized);
            }

            try
            {
                await _commentRepository.DeleteAsync(comment);
                await _commentRepository.SaveChangesAsync();
                
                var response = new DeleteCommentResponseViewModel
                {
                    Id = request.Id,
                    Message = "Comment deleted successfully"
                };
                
                return RequestResult<DeleteCommentResponseViewModel>.Success(
                    data: response, 
                    message: "Comment deleted successfully");
            }
            catch (Exception)
            {
                return RequestResult<DeleteCommentResponseViewModel>.Failure(ErrorCode.DatabaseError);
            }
        }
    }
}


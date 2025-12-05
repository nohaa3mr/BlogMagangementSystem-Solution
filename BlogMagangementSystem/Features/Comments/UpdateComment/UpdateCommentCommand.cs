using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.Comments.UpdateComment;
using Mapster;

namespace BlogMagangementSystem.Features.Comments.UpdateComment
{
    public sealed record UpdateCommentCommand(Guid Id, string Content, Guid UserId) : IRequest<RequestResult<UpdateCommentResponseViewModel>>;

    public class UpdateCommentCommandHandler : BaseRequestHandler<UpdateCommentCommand, RequestResult<UpdateCommentResponseViewModel>>
    {
        private readonly GenericRepository<Comment> _commentRepository;

        public UpdateCommentCommandHandler(
            BaseRequestParameters parameters,
            GenericRepository<Comment> commentRepository) : base(parameters)
        {
            _commentRepository = commentRepository;
        }

        public override async Task<RequestResult<UpdateCommentResponseViewModel>> Handle(
            UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);
            
            if (comment is null || comment.IsDeleted || !comment.IsActive)
                return RequestResult<UpdateCommentResponseViewModel>.Failure(ErrorCode.DoesNotExist);

            // Check if user owns the comment or is admin
            if (comment.UserId != request.UserId)
            {
                // Check if user is admin
                // This would require getting user role from token - simplified for now
                return RequestResult<UpdateCommentResponseViewModel>.Failure(ErrorCode.NotAuthorized);
            }

            try
            {
                comment.Content = request.Content;
                comment.UpdatedAt = DateTime.UtcNow;
                
                await _commentRepository.UpdateInclude(comment, nameof(Comment.Content), nameof(BaseEntity.UpdatedAt));
                
                var response = comment.Adapt<UpdateCommentResponseViewModel>();
                return RequestResult<UpdateCommentResponseViewModel>.Success(
                    data: response, 
                    message: "Comment updated successfully");
            }
            catch (Exception)
            {
                return RequestResult<UpdateCommentResponseViewModel>.Failure(ErrorCode.DatabaseError);
            }
        }
    }
}


using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.Comments.GetCommentById;
using Mapster;

namespace BlogMagangementSystem.Features.Comments.GetCommentById
{
    public sealed record GetCommentByIdQuery(Guid Id) : IRequest<RequestResult<GetCommentByIdResponseViewModel>>;

    public class GetCommentByIdQueryHandler : BaseRequestHandler<GetCommentByIdQuery, RequestResult<GetCommentByIdResponseViewModel>>
    {
        private readonly GenericRepository<Comment> _commentRepository;

        public GetCommentByIdQueryHandler(
            BaseRequestParameters parameters,
            GenericRepository<Comment> commentRepository) : base(parameters)
        {
            _commentRepository = commentRepository;
        }

        public override async Task<RequestResult<GetCommentByIdResponseViewModel>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);
            
            if (comment is null || comment.IsDeleted || !comment.IsActive)
                return RequestResult<GetCommentByIdResponseViewModel>.Failure(ErrorCode.DoesNotExist);

            var response = comment.Adapt<GetCommentByIdResponseViewModel>();
            return RequestResult<GetCommentByIdResponseViewModel>.Success(data: response, message: "Comment retrieved successfully");
        }
    }
}


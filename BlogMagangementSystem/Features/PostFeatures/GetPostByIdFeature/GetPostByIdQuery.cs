using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.PostFeatures.GetPostByIdFeature
{
    public sealed record GetPostByIdQuery(Guid PostId) : IRequest<RequestResult<PostDTO>>;
    public class GetPostByIdQueryHandler : BaseRequestHandler<GetPostByIdQuery , RequestResult<PostDTO>>
    {
        private readonly GenericRepository<Post> _repository;

        public GetPostByIdQueryHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters)
        {
            _repository = repository;
        }
        public override async Task<RequestResult<PostDTO>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.PostId == Guid.Empty)
            {
                return RequestResult<PostDTO>.Failure(ErrorCode.PostNotFound);
            }
            try
            {
                var post = request.PostId != Guid.Empty  ? await _repository.GetByIdAsync(request.PostId) : null;
                if (post is null || post.IsDeleted)
                {
                    return RequestResult<PostDTO>.Failure(ErrorCode.PostNotFound);
                }
                var result = post.Adapt<PostDTO>(); 
                return RequestResult<PostDTO>.Success(data: result, message: "Post Retrieved Successfully");

            }
            catch (Exception)
            {
                return RequestResult<PostDTO>.Failure(ErrorCode.DatabaseError);
            }

        }
    }

}

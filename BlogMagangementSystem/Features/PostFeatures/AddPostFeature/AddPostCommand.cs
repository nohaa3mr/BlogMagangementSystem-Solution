using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.PostFeatures.AddPostFeature
{
    public sealed record AddPostCommand(AddPostDto Dto) : IRequest<RequestResult<AddPostDto>>;
    public  class AddPostCommandHandler  : BaseRequestHandler<AddPostCommand , RequestResult<AddPostDto>>
    {

        private readonly GenericRepository<Post> _repository;
        public AddPostCommandHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters )
        {
            _repository = repository;
        } 
        
        public override async Task<RequestResult<AddPostDto>> Handle(AddPostCommand request, CancellationToken cancellationToken)
        { 
            if (request.Dto is null)
            return RequestResult<AddPostDto>.Failure(ErrorCode.InvalidInput);

            try
            {
                var Post = request.Dto.Adapt<Post>();
                await _repository.AddAsync(Post);
                await _repository.SaveChangesAsync();
                var result = Post.Adapt<AddPostDto>();
                return RequestResult<AddPostDto>.Success(data: result, message: "Post Added Successfully");
            }
            catch (Exception)
            {

                return RequestResult<AddPostDto>.Failure(ErrorCode.DatabaseError);
            }


        }

    }

}

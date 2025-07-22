using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using BlogMagangementSystem.Features.GetPostByIdFeature;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.DeletePostFeature
{
    public sealed record DeletePostByIdCommand(int Id) : IRequest<RequestResult<PostDTO>>;
    public class DeletePostByIdCommandHandler : BaseRequestHandler<DeletePostByIdCommand, RequestResult<PostDTO>>
    {
        private readonly GenericRepository<Post> _repository;

        public DeletePostByIdCommandHandler(BaseRequestParameters parameters ,GenericRepository<Post>  repository) : base(parameters)
        {
            _repository = repository;
        }
        public override async Task<RequestResult<PostDTO>> Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return RequestResult<PostDTO>.Failure(ErrorCode.InvalidInput);
            var post = await _mediator.Send(new GetPostByIdQuery(request.Id), cancellationToken);
            if (!post.IsSuccess)
                return RequestResult<PostDTO>.Failure(post.ErrorCode);

            var postToDelete = post.Data.Adapt<Post>();
             await _repository.DeleteAsync(postToDelete);
            await _repository.SaveChangesAsync();
            var result = post.Data.Adapt<PostDTO>();
            return RequestResult<PostDTO>.Success(data:result, message:"Post Deleted Successfully!");
        }
    }


}

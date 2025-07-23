using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.GetPostByIdFeature;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.PostFeatures.UpdatePostFeature
{
    public sealed record UpdatePostByIdCommand(UpdatePostDto DTO) :IRequest<RequestResult<UpdatePostDto>>;
    public class UpdatePostByIdCommandHandler : BaseRequestHandler<UpdatePostByIdCommand, RequestResult<UpdatePostDto>>
    {
        private readonly GenericRepository<Post> _repository;

        public UpdatePostByIdCommandHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters)
        {
            _repository = repository;
        }
        public override async Task<RequestResult<UpdatePostDto>> Handle(UpdatePostByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.DTO is null||request.DTO.Id <= 0)
            {
                return RequestResult<UpdatePostDto>.Failure(ErrorCode.InvalidInput);
            }            
            try
            {
                var QueryResult = await _mediator.Send(new GetPostByIdQuery(request.DTO.Id));

                if (!QueryResult.IsSuccess)
                {
                    return RequestResult<UpdatePostDto>.Failure(ErrorCode.PostNotFound);
                }

                var Post = QueryResult.Data.Adapt<Post>();
                await _repository.UpdateInclude(Post);
                var result = Post.Adapt<UpdatePostDto>();
                return RequestResult<UpdatePostDto>.Success
                (
                    data: result, 
                    message: "Post Updated Successfully"
                );
            }
            catch (Exception)
            {
                return RequestResult<UpdatePostDto>.Failure(ErrorCode.DatabaseError);
            }
        }
    }


}

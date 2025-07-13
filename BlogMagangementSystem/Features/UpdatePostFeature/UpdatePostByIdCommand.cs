using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.GetPostByIdFeature;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public sealed record UpdatePostByIdCommand(UpdatePostDto Dto) :IRequest<RequestResult<UpdatePostDto>>;
    public class UpdatePostByIdCommandHandler : BaseRequestHandler<UpdatePostByIdCommand, RequestResult<UpdatePostDto>>
    {
        private readonly GenericRepository<Post> _repository;

        public UpdatePostByIdCommandHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters)
        {
            _repository = repository;
        }
        public override async Task<RequestResult<UpdatePostDto>> Handle(UpdatePostByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.Dto is null || request.Dto.Id <= 0)
            
                return RequestResult<UpdatePostDto>.Failure(ErrorCode.InvalidInput);
            
            try
            {
                var QueryResult = await _mediator.Send(new GetPostByIdQuery(request.Dto.Id));

                if (!QueryResult.IsSuccess)
                {
                    return RequestResult<UpdatePostDto>.Failure(ErrorCode.PostNotFound);
                }
                var updatedPost = request.Dto.Adapt<Post>();
                await _repository.UpdateInclude(updatedPost, updatedPost.Content , updatedPost.Title );
                var result = QueryResult.Adapt<UpdatePostDto>();
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

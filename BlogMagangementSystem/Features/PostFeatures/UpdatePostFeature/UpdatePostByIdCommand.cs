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
            if (request.DTO is null||request.DTO.ID == Guid.Empty)
            {
                return RequestResult<UpdatePostDto>.Failure(ErrorCode.InvalidInput);
            }            
            try
            {
                var QueryResult = await _mediator.Send(new GetPostByIdQuery(request.DTO.ID));

                if (!QueryResult.IsSuccess)
                {
                    return RequestResult<UpdatePostDto>.Failure(ErrorCode.PostNotFound);
                }

                var post = QueryResult.Data.Adapt<Post>();
                await _repository.UpdateAsync(post);
                var result = post.Adapt<UpdatePostDto>();
                return RequestResult<UpdatePostDto>.Success
                (
                    data: result, 
                    message: "Post Updated Successfully"
                );
            }
            catch (Exception)
            {
                return RequestResult<UpdatePostDto>.Failure(ErrorCode.InvalidPostData);
            }
        }
    }


}

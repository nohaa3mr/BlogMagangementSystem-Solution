namespace BlogMagangementSystem.Features.PostFeatures.DeletePostFeature
{
    public sealed record DeletePostByIdCommand(PostDTO DTO) : IRequest<RequestResult<PostDTO>>;
    public class DeletePostByIdCommandHandler : BaseRequestHandler<DeletePostByIdCommand, RequestResult<PostDTO>>
    {
        private readonly GenericRepository<Post> _repository;

        public DeletePostByIdCommandHandler(BaseRequestParameters parameters ,GenericRepository<Post>  repository) : base(parameters)
        {
            _repository = repository;
        }
        public override async Task<RequestResult<PostDTO>> Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.DTO.ID  == Guid.Empty) return RequestResult<PostDTO>.Failure(ErrorCode.InvalidInput);
            var POST = request.DTO.Adapt<Post>();
            var post = await _repository.GetByIdAsync(POST.ID);
            if (post is null)
                return RequestResult<PostDTO>.Failure(ErrorCode.PostNotFound);
            await _repository.DeleteAsync(post);
            var response = post.Adapt<PostDTO>();
            return RequestResult<PostDTO>.Success(data: response, message: "Post Deleted Successfully");

        }
    }


}

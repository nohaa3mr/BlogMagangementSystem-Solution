namespace BlogMagangementSystem.Orchestrators.AddPostOrchastrator
{
    public sealed record AddPostOrchestrator(AddPostDto Dto) : IRequest<RequestResult<AddPostDto>>;
    public class AddPostOrchastratorHandler : BaseRequestHandler<AddPostOrchestrator, RequestResult<AddPostDto>>
    {
        private readonly BaseRequestParameters parameters;

        public AddPostOrchastratorHandler(BaseRequestParameters parameters) : base(parameters)
        {
            this.parameters = parameters;
        }
        public override async Task<RequestResult<AddPostDto>> Handle(AddPostOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.Dto is null)
                return RequestResult<AddPostDto>.Failure(ErrorCode.InvalidInput);
            await parameters.Mediator.Send(new AddPostCommand(request.Dto));
      
           await parameters.CapPublisher.PublishAsync("post.Iscreated",new {request.Dto.Title , request.Dto.Content , request.Dto.Message});
            return RequestResult<AddPostDto>.Success(data: request.Dto, message: "Post Added Successfully");
        }
    }
}

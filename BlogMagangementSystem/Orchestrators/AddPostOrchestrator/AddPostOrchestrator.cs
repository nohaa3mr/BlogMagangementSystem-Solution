using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.PostModule;
using DotNetCore.CAP;
using MediatR;

namespace BlogMagangementSystem.Orchestrators.AddPostOrchastrator
{
    public sealed record AddPostOrchestrator(AddPostDto Dto) : IRequest<RequestResult<AddPostDto>>;
    public class AddPostOrchastratorHandler : BaseRequestHandler<AddPostOrchestrator, RequestResult<AddPostDto>>
    {
        private readonly ICapPublisher _capPublisher;

        public AddPostOrchastratorHandler(BaseRequestParameters parameters,ICapPublisher capPublisher) : base(parameters)
        {
            this._capPublisher = capPublisher;
        }
        public override async Task<RequestResult<AddPostDto>> Handle(AddPostOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.Dto is null)
                return RequestResult<AddPostDto>.Failure(ErrorCode.InvalidInput);
            await _mediator.Send(new AddPostCommand(request.Dto));
      
          //  await _capPublisher.PublishAsync("post.Iscreated",new {request.Dto.Title , request.Dto.Content , request.Dto.Message});
            return RequestResult<AddPostDto>.Success(data: request.Dto, message: "Post Added Successfully");
        }
    }
}

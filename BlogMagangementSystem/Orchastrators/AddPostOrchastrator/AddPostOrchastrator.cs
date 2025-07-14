using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.MessageBroker;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.PostModule;
using DotNetCore.CAP;
using MediatR;

namespace BlogMagangementSystem.Orchastrators.AddPostOrchastrator
{
    public sealed record AddPostOrchastrator(AddPostDto Dto) : IRequest<RequestResult<AddPostDto>>;
    public class AddPostOrchastratorHandler : BaseRequestHandler<AddPostOrchastrator, RequestResult<AddPostDto>>
    {
        private readonly ICapPublisher _capPublisher;

        public AddPostOrchastratorHandler(BaseRequestParameters parameters,ICapPublisher capPublisher) : base(parameters)
        {
            this._capPublisher = capPublisher;
        }
        public override async Task<RequestResult<AddPostDto>> Handle(AddPostOrchastrator request, CancellationToken cancellationToken)
        {
            if (request.Dto is null)
                return RequestResult<AddPostDto>.Failure(ErrorCode.InvalidInput);
            await _mediator.Send(new AddPostCommand(request.Dto));
            if (!request.Dto.IsSendToMessageBroker)
            {
                await _mediator.Send(new MessageBrokerCommand(request.Dto.Title));
            }
            await _capPublisher.PublishAsync("post.created",new {request.Dto.Title , request.Dto.Content});
            return RequestResult<AddPostDto>.Success(data: request.Dto, message: "Post Added Successfully");
        }
    }
}

using BlogMagangementSystem.Features.Comments.CreateComment.DTOs;

namespace BlogMagangementSystem.Features.Comments.CreateComment.Commands
{
    public sealed record CreateCommentOrchestrator(CreateCommentRequestDTO DTO) : IRequest<RequestResult<CreateCommentResponseDTO>>;
    
    public class CreateCommentOrchestratorHandler : BaseRequestHandler<CreateCommentOrchestrator, RequestResult<CreateCommentResponseDTO>>
    {
        private readonly BaseRequestParameters _parameters;

        public CreateCommentOrchestratorHandler(BaseRequestParameters parameters) : base(parameters)
        {
            _parameters = parameters;
        }

        public override async Task<RequestResult<CreateCommentResponseDTO>> Handle(CreateCommentOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.DTO is null)
                return RequestResult<CreateCommentResponseDTO>.Failure(ErrorCode.InvalidInput);
            
            var result = await _parameters.Mediator.Send(new CreateCommentCommand(request.DTO));
            if (!result.IsSuccess)
            {
                return RequestResult<CreateCommentResponseDTO>.Failure(result.ErrorCode);
            }
            
            return RequestResult<CreateCommentResponseDTO>.Success(data: result.Data, message: result.Message);
        }
    }
}

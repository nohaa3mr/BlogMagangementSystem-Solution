using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.Comments.CreateComment.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.Comments.CreateComment
{
    [Authorize(Policy = "Authenticated")]
    public class CreateCommentEndpoint : BaseEndpoint<CreateCommentRequestViewModel, CreateCommentResponseViewModel>
    {
        public CreateCommentEndpoint(BaseEndpointParameters<CreateCommentRequestViewModel> parameters) : base(parameters) { }

        [HttpPost("Comment/Create")]
        public async Task<EndpointResponse<CreateCommentResponseViewModel>> CreateComment([FromBody] CreateCommentRequestViewModel request)
        {
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<CreateCommentResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            // Get UserId from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return EndpointResponse<CreateCommentResponseViewModel>.Failure(ErrorCode.NotAuthorized, "Invalid user token");
            }

            var dto = request.Adapt<DTOs.CreateCommentRequestDTO>();
            dto.UserId = userId;

            var result = await Mediator.Send(new Commands.CreateCommentOrchestrator(dto));
            if (!result.IsSuccess)
            {
                return EndpointResponse<CreateCommentResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<CreateCommentResponseViewModel>();
            return EndpointResponse<CreateCommentResponseViewModel>.Success(response, result.Message);
        }
    }
}


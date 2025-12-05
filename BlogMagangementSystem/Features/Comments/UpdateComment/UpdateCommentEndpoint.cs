using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.Comments.UpdateComment;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.Comments.UpdateComment
{
    [Authorize(Policy = "Authenticated")]
    public class UpdateCommentEndpoint : BaseEndpoint<UpdateCommentRequestViewModel, UpdateCommentResponseViewModel>
    {
        public UpdateCommentEndpoint(BaseEndpointParameters<UpdateCommentRequestViewModel> parameters) : base(parameters) { }

        [HttpPut("Comment/Update")]
        public async Task<EndpointResponse<UpdateCommentResponseViewModel>> UpdateComment([FromBody] UpdateCommentRequestViewModel request)
        {
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<UpdateCommentResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            // Get UserId from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return EndpointResponse<UpdateCommentResponseViewModel>.Failure(ErrorCode.NotAuthorized, "Invalid user token");
            }

            var result = await Mediator.Send(new UpdateCommentCommand(request.Id, request.Content, userId));
            if (!result.IsSuccess)
            {
                return EndpointResponse<UpdateCommentResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<UpdateCommentResponseViewModel>();
            return EndpointResponse<UpdateCommentResponseViewModel>.Success(response, result.Message);
        }
    }
}


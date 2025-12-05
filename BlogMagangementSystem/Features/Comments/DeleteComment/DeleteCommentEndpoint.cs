using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.Comments.DeleteComment;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.Comments.DeleteComment
{
    [Authorize(Policy = "Authenticated")]
    public class DeleteCommentEndpoint : BaseEndpoint<DeleteCommentRequestViewModel, DeleteCommentResponseViewModel>
    {
        public DeleteCommentEndpoint(BaseEndpointParameters<DeleteCommentRequestViewModel> parameters) : base(parameters) { }

        [HttpDelete("Comment/Delete/{Id}")]
        public async Task<EndpointResponse<DeleteCommentResponseViewModel>> DeleteComment([FromRoute] Guid Id)
        {
            var request = new DeleteCommentRequestViewModel { Id = Id };
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<DeleteCommentResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            // Get UserId from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return EndpointResponse<DeleteCommentResponseViewModel>.Failure(ErrorCode.NotAuthorized, "Invalid user token");
            }

            var result = await Mediator.Send(new DeleteCommentCommand(Id, userId));
            if (!result.IsSuccess)
            {
                return EndpointResponse<DeleteCommentResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<DeleteCommentResponseViewModel>();
            return EndpointResponse<DeleteCommentResponseViewModel>.Success(response, result.Message);
        }
    }
}


using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.UserFeatures.DeleteUser;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogMagangementSystem.Features.UserFeatures.DeleteUser
{
    [Authorize(Policy = "Authenticated")]
    public class DeleteUserEndpoint : BaseEndpoint<DeleteUserRequestViewModel, DeleteUserResponseViewModel>
    {
        public DeleteUserEndpoint(BaseEndpointParameters<DeleteUserRequestViewModel> parameters) : base(parameters) { }

        [HttpDelete("User/Delete/{Id}")]
        public async Task<EndpointResponse<DeleteUserResponseViewModel>> DeleteUser([FromRoute] Guid Id)
        {
            var request = new DeleteUserRequestViewModel { Id = Id };
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<DeleteUserResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            // Get UserId from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return EndpointResponse<DeleteUserResponseViewModel>.Failure(ErrorCode.NotAuthorized, "Invalid user token");
            }

            // Check if user is admin
            var isAdmin = User.IsInRole("Admin");

            var result = await Mediator.Send(new DeleteUserCommand(Id, userId, isAdmin));
            if (!result.IsSuccess)
            {
                return EndpointResponse<DeleteUserResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<DeleteUserResponseViewModel>();
            return EndpointResponse<DeleteUserResponseViewModel>.Success(response, result.Message);
        }
    }
}


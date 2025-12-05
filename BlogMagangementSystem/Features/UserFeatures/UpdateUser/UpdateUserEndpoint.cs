using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.UserFeatures.UpdateUser;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogMagangementSystem.Features.UserFeatures.UpdateUser
{
    [Authorize(Policy = "Authenticated")]
    public class UpdateUserEndpoint : BaseEndpoint<UpdateUserRequestViewModel, UpdateUserResponseViewModel>
    {
        public UpdateUserEndpoint(BaseEndpointParameters<UpdateUserRequestViewModel> parameters) : base(parameters) { }

        [HttpPut("User/Update")]
        public async Task<EndpointResponse<UpdateUserResponseViewModel>> UpdateUser([FromBody] UpdateUserRequestViewModel request)
        {
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<UpdateUserResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            // Get UserId from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return EndpointResponse<UpdateUserResponseViewModel>.Failure(ErrorCode.NotAuthorized, "Invalid user token");
            }

            // Check if user is admin
            var isAdmin = User.IsInRole("Admin");

            var result = await Mediator.Send(new UpdateUserCommand(request.Id, request, userId, isAdmin));
            if (!result.IsSuccess)
            {
                return EndpointResponse<UpdateUserResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<UpdateUserResponseViewModel>();
            return EndpointResponse<UpdateUserResponseViewModel>.Success(response, result.Message);
        }
    }
}


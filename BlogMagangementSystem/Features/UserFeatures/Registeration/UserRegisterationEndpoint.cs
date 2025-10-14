using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.UserFeatures.Registeration
{
    public class UserRegisterationEndpoint : BaseEndpoint<UserRegisterationRequestViewModel, UserRegisterationResponseViewModel>
    {
        public UserRegisterationEndpoint(BaseEndpointParameters<UserRegisterationRequestViewModel> parameters):base(parameters){}

        [HttpPost("User/Registration")]

        public async Task<EndpointResponse<UserRegisterationResponseViewModel>> RegisterUserAsync(UserRegisterationRequestViewModel requestViewModel)
        {
            var validationResult = await ValidateAsync(requestViewModel);
            if(!validationResult.IsSuccess)
            {
                return EndpointResponse<UserRegisterationResponseViewModel>.Failure(validationResult.ErrorCode);
            }
            var command = requestViewModel.Adapt<UserDto>();
            var Result = await Mediator.Send(new UserRegisterationCommand(command));
            if (!Result.IsSuccess)
            {
                return EndpointResponse<UserRegisterationResponseViewModel>.Failure(Result.ErrorCode);
            }
            var response = Result.Data.Adapt<UserRegisterationResponseViewModel>(); 
            return EndpointResponse<UserRegisterationResponseViewModel>.Success(response,Result.Message);
        }
    }
}

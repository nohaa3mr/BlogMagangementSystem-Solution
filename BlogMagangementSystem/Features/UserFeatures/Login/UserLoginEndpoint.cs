using BlogMagangementSystem.Common.Structures.ResponseStructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.UserFeatures.Login
{
    public class UserLoginEndpoint : BaseEndpoint<UserLoginRequestViewModel,UserLoginResponseViewModel>
    {
        public UserLoginEndpoint(BaseEndpointParameters<UserLoginRequestViewModel> parameters) : base(parameters)
        {
            
        }
        [HttpPost("User/Login")]
        public async Task<EndpointResponse<UserLoginResponseViewModel>> Login(UserLoginRequestViewModel model)
        {
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<UserLoginResponseViewModel>.Failure(validationResult.ErrorCode , validationResult.Message);
            }
            var command = model.Adapt<UserLoginDTO>();
            var result = await Mediator.Send(new UserLoginCommand(command));
            if (!result.IsSuccess)
            {
                return EndpointResponse<UserLoginResponseViewModel>.Failure(result.ErrorCode);
            }
            var response = result.Data.Adapt<UserLoginResponseViewModel>();
            return EndpointResponse<UserLoginResponseViewModel>.Success(response, result.Message);

        }
    }
}

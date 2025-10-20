using BlogMagangementSystem.Features.UserFeatures.GetUserRoleByUserID.Query;

namespace BlogMagangementSystem.Features.UserFeatures.GetUserRoleByUserID
{
    public class GetUserRoleByUserIDEndpoint : BaseEndpoint<GetUserRoleByIDRequestViewModel,GetUserRoleByIDResponseViewModel>
    {
        public GetUserRoleByUserIDEndpoint(BaseEndpointParameters<GetUserRoleByIDRequestViewModel> parameters) : base(parameters){}

        [HttpGet("User/GetRoleByUserID")]
        public async Task<EndpointResponse<GetUserRoleByIDResponseViewModel>> GetRoleByUserID([FromQuery] GetUserRoleByIDRequestViewModel model)
        {
            var validationResult = await ValidateAsync(model);
            if(!validationResult.IsSuccess)
                return EndpointResponse<GetUserRoleByIDResponseViewModel>.Failure(validationResult.ErrorCode , validationResult.Message);
            var query = await Mediator.Send(new GetUserRoleByIDQuery(model.ID));
            if (!query.IsSuccess)
                return EndpointResponse<GetUserRoleByIDResponseViewModel>.Failure(query.ErrorCode , query.Message);
            var response = query.Data.Adapt<GetUserRoleByIDResponseViewModel>();
            return EndpointResponse<GetUserRoleByIDResponseViewModel>.Success(response , "User Role Retrieved Successfully");

        } 
    }
}

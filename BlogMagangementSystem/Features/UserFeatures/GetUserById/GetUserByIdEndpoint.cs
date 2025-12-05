using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.UserFeatures.GetUserById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.UserFeatures.GetUserById
{
    [Authorize(Policy = "Authenticated")]
    public class GetUserByIdEndpoint : BaseEndpoint<GetUserByIdRequestViewModel, GetUserByIdResponseViewModel>
    {
        public GetUserByIdEndpoint(BaseEndpointParameters<GetUserByIdRequestViewModel> parameters) : base(parameters) { }

        [HttpGet("User/GetById/{Id}")]
        public async Task<EndpointResponse<GetUserByIdResponseViewModel>> GetUserById([FromRoute] Guid Id)
        {
            var request = new GetUserByIdRequestViewModel { Id = Id };
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<GetUserByIdResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            var result = await Mediator.Send(new GetUserByIdQuery(Id));
            if (!result.IsSuccess)
            {
                return EndpointResponse<GetUserByIdResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<GetUserByIdResponseViewModel>();
            return EndpointResponse<GetUserByIdResponseViewModel>.Success(response, result.Message);
        }
    }
}


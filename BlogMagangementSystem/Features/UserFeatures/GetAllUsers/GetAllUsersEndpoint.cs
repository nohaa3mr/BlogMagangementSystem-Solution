using BlogMagangementSystem.Common.Helpers;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.UserFeatures.GetAllUsers;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.UserFeatures.GetAllUsers
{
    [Authorize(Policy = "AdminOnly")]
    public class GetAllUsersEndpoint : BaseEndpoint<GetAllUsersRequestViewModel, PagingHelper<GetAllUsersResponseViewModel>>
    {
        public GetAllUsersEndpoint(BaseEndpointParameters<GetAllUsersRequestViewModel> parameters) : base(parameters) { }

        [HttpGet("User/GetAll")]
        public async Task<EndpointResponse<PagingHelper<GetAllUsersResponseViewModel>>> GetAllUsers([FromQuery] GetAllUsersRequestViewModel model)
        {
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<PagingHelper<GetAllUsersResponseViewModel>>.Failure(
                    validationResult.ErrorCode, validationResult.Message);
            }

            var result = await Mediator.Send(new GetAllUsersQuery(
                model.PageIndex, 
                model.PageSize, 
                model.SortAsc, 
                model.SearchTerm));

            if (!result.IsSuccess)
            {
                return EndpointResponse<PagingHelper<GetAllUsersResponseViewModel>>.Failure(
                    result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<PagingHelper<GetAllUsersResponseViewModel>>();
            return EndpointResponse<PagingHelper<GetAllUsersResponseViewModel>>.Success(response, result.Message);
        }
    }
}


using BlogMagangementSystem.Common.Helpers;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.Comments.GetAllComments;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.Comments.GetAllComments
{
    [Authorize(Policy = "Authenticated")]
    public class GetAllCommentsEndpoint : BaseEndpoint<GetAllCommentsRequestViewModel, PagingHelper<GetAllCommentsResponseViewModel>>
    {
        public GetAllCommentsEndpoint(BaseEndpointParameters<GetAllCommentsRequestViewModel> parameters) : base(parameters) { }

        [HttpGet("Comment/GetAll")]
        public async Task<EndpointResponse<PagingHelper<GetAllCommentsResponseViewModel>>> GetAllComments([FromQuery] GetAllCommentsRequestViewModel model)
        {
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<PagingHelper<GetAllCommentsResponseViewModel>>.Failure(
                    validationResult.ErrorCode, validationResult.Message);
            }

            var result = await Mediator.Send(new GetAllCommentsQuery(
                model.PageIndex, 
                model.PageSize, 
                model.SortAsc, 
                model.PostId));

            if (!result.IsSuccess)
            {
                return EndpointResponse<PagingHelper<GetAllCommentsResponseViewModel>>.Failure(
                    result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<PagingHelper<GetAllCommentsResponseViewModel>>();
            return EndpointResponse<PagingHelper<GetAllCommentsResponseViewModel>>.Success(response, result.Message);
        }
    }
}


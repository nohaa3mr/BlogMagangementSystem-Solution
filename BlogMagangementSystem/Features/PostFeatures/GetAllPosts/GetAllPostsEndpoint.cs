using BlogMagangementSystem.Common.Helpers;
using BlogMagangementSystem.Features.PostFeatures.GetAllPosts.Query;
using Microsoft.AspNetCore.Authorization;

namespace BlogMagangementSystem.Features.PostFeatures.GetAllPosts;

[Authorize(Policy = "Authenticated")]
public class GetAllPostsEndpoint : BaseEndpoint<GetAllPostsRequestViewModel , PagingHelper<GetAllPostsResponseViewModel>>
{
    public GetAllPostsEndpoint(BaseEndpointParameters<GetAllPostsRequestViewModel> parameters): base(parameters)
    {
        
    }
    [HttpGet("posts/getall")]
    public async Task<EndpointResponse<PagingHelper<GetAllPostsResponseViewModel>>> GetAll([FromQuery] GetAllPostsRequestViewModel model)
    {
        var validationResult = await ValidateAsync(model);
        if(!validationResult.IsSuccess)
            return EndpointResponse<PagingHelper<GetAllPostsResponseViewModel>>.Failure(validationResult.ErrorCode, validationResult.Message);
        var query = await Mediator.Send(new GetAllPostsQuery(model.PageIndex,model.PageSize ,model.SortAsc,model.SearchTerm));
        if(!query.IsSuccess)
            return EndpointResponse<PagingHelper<GetAllPostsResponseViewModel>>.Failure(query.ErrorCode, query.Message);
        var response = query.Data.Adapt<PagingHelper<GetAllPostsResponseViewModel>>();
        return EndpointResponse<PagingHelper<GetAllPostsResponseViewModel>>.Success(response, query.Message);
    }
}

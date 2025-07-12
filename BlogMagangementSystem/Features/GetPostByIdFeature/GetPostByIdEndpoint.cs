using BlogMagangementSystem.Common.Structures.ResponseStructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.GetPostByIdFeature
{
    public class GetPostByIdEndpoint : BaseEndpoint<GetPostByIdRequestViewModel, GetPostByIdResponseViewModel>
    {
        public GetPostByIdEndpoint(BaseEndpointParameters<GetPostByIdRequestViewModel> parameters) : base(parameters)
        {
        }
        [HttpGet("GetPostById/{postId}")]
        public async Task<EndpointResponse<GetPostByIdResponseViewModel>> GetPostByIdAsync(int postId)
        {
            var result = await Mediator.Send(new GetPostByIdQuery(postId));
            var response = result.Adapt<EndpointResponse<GetPostByIdResponseViewModel>>();
            if (!response.IsSuccess)
            {
                return EndpointResponse<GetPostByIdResponseViewModel>.Failure(response.ErrorCode, response.Message);
            }
            var successResponse = response.Data.Adapt<GetPostByIdResponseViewModel>();
            return EndpointResponse<GetPostByIdResponseViewModel>.Success(successResponse);
        }

        
    }
    
    
}

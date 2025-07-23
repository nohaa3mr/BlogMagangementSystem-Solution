using BlogMagangementSystem.Common.Structures.ResponseStructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.PostFeatures.GetPostByIdFeature
{
    public class GetPostByIdEndpoint : BaseEndpoint<GetPostByIdRequestViewModel, GetPostByIdResponseViewModel>
    {
        public GetPostByIdEndpoint(BaseEndpointParameters<GetPostByIdRequestViewModel> parameters) : base(parameters)
        {
        }
        [HttpGet("GetPostById/{PostId}")]
        public async Task<EndpointResponse<GetPostByIdResponseViewModel>> GetPostByIdAsync(int PostId)
        {
            var RequestViewModel = new GetPostByIdRequestViewModel { Id = PostId };
            var validationResponse = await ValidateAsync(RequestViewModel);
            if (!validationResponse.IsSuccess)
            {
                return EndpointResponse<GetPostByIdResponseViewModel>.Failure(validationResponse.ErrorCode, validationResponse.Message);
            }
            var result = await Mediator.Send(new GetPostByIdQuery(PostId));
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

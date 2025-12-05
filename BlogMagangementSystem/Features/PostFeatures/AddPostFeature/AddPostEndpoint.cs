using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.PostFeatures.AddPostFeature.Commands;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.PostFeatures.AddPostFeature
{
    [Authorize(Policy = "Authenticated")]
    public class AddPostEndpoint :BaseEndpoint<AddPostRequestViewModel,AddPostResponseViewModel>
    {
        public AddPostEndpoint(BaseEndpointParameters<AddPostRequestViewModel> parameters ) :base(parameters){ }

        [HttpPost("Post/Add")]

        public async Task<EndpointResponse<AddPostResponseViewModel>> AddPostAsync( AddPostRequestViewModel request)
        {
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<AddPostResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }
            var command = request.Adapt<AddPostDto>();
            var result = await Mediator.Send(new AddPostOrchestrator(command));
            var response =  result.Adapt<EndpointResponse<AddPostResponseViewModel>>();

            if (!response.IsSuccess)
            {
                return EndpointResponse<AddPostResponseViewModel>.Failure(response.ErrorCode, response.Message);
            }
            var SuccessResponse = response.Data.Adapt<AddPostResponseViewModel>();
            return EndpointResponse<AddPostResponseViewModel>.Success(SuccessResponse);
        }

    }
}

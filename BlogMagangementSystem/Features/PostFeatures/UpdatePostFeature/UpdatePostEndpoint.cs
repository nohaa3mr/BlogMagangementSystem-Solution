using BlogMagangementSystem.Common.Structures.ResponseStructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.PostFeatures.UpdatePostFeature
{
    public class UpdatePostEndpoint :BaseEndpoint<UpdatePostRequestViewModel,UpdatePostResponseViewModel>
    {
        public UpdatePostEndpoint(BaseEndpointParameters<UpdatePostRequestViewModel> parameters): base(parameters){}
        [HttpPut("Post/Update")]
        public async Task<EndpointResponse<UpdatePostResponseViewModel>> UpdatePostAsync([FromBody]UpdatePostRequestViewModel requestViewModel)
        { 
            var validationResult = await ValidateAsync(requestViewModel);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<UpdatePostResponseViewModel>.Failure(validationResult.ErrorCode);
            }
          
            var command = requestViewModel.Adapt<UpdatePostDto>();
            var result = await Mediator.Send(new UpdatePostByIdCommand(command));
            var response = requestViewModel.Adapt<UpdatePostResponseViewModel>();
            return EndpointResponse<UpdatePostResponseViewModel>.Success(response , result.Message);
        }
    }
}

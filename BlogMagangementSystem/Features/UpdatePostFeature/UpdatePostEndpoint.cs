using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.GetPostByIdFeature;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public class UpdatePostEndpoint :BaseEndpoint<UpdatePostRequestViewModel,UpdatePostResponseViewModel>
    {
        public UpdatePostEndpoint(BaseEndpointParameters<UpdatePostRequestViewModel> parameters): base(parameters){}
        [HttpPut("UpdatePost/")]
        public async Task<EndpointResponse<UpdatePostResponseViewModel>> UpdatePostAsync(UpdatePostRequestViewModel requestViewModel)
        { 

            var request = requestViewModel.Adapt<UpdatePostRequestViewModel>();
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<UpdatePostResponseViewModel>.Failure(validationResult.ErrorCode);
            }
          
            var command = requestViewModel.Adapt<UpdatePostDto>();
            var result = await Mediator.Send(new UpdatePostByIdCommand(command));
            var response = request.Adapt<UpdatePostResponseViewModel>();
   

            return EndpointResponse<UpdatePostResponseViewModel>.Success(response);
        }
    }
}

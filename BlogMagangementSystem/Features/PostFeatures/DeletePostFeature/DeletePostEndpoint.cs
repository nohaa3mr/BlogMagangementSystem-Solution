using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.PostFeatures.DeletePostFeature
{
    public class DeletePostEndpoint : BaseEndpoint<DeletePostRequestViewModel,DeletePostResponseViewModel>
    {
        public DeletePostEndpoint(BaseEndpointParameters<DeletePostRequestViewModel> parameters): base(parameters){ }

        [HttpDelete("Delete-Post-By-Id")]
        public async Task<EndpointResponse<DeletePostResponseViewModel>> DeletePost(DeletePostRequestViewModel model )
        {  

            var validationResponse = await ValidateAsync(model);
            if(!validationResponse.IsSuccess)
            {
                return EndpointResponse<DeletePostResponseViewModel>.Failure(validationResponse.ErrorCode);
            }
            var command = model.Adapt<PostDTO>();
            var result = await Mediator.Send(new DeletePostByIdCommand(command));
            var RequestResult = result.Adapt<EndpointResponse<DeletePostResponseViewModel>>();
            if (!RequestResult.IsSuccess)
            {
                return EndpointResponse<DeletePostResponseViewModel>.Failure(RequestResult.ErrorCode);
            }
            var response = RequestResult.Data.Adapt<DeletePostResponseViewModel>();
            return EndpointResponse<DeletePostResponseViewModel>.Success(data:response,message: RequestResult.Message);
        }
    }
}

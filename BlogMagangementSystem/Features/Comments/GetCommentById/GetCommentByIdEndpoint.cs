using BlogMagangementSystem.Common.Structures.ResponseStructure;
using BlogMagangementSystem.Features.Comments.GetCommentById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.Comments.GetCommentById
{
    [Authorize(Policy = "Authenticated")]
    public class GetCommentByIdEndpoint : BaseEndpoint<GetCommentByIdRequestViewModel, GetCommentByIdResponseViewModel>
    {
        public GetCommentByIdEndpoint(BaseEndpointParameters<GetCommentByIdRequestViewModel> parameters) : base(parameters) { }

        [HttpGet("Comment/GetById/{Id}")]
        public async Task<EndpointResponse<GetCommentByIdResponseViewModel>> GetCommentById([FromRoute] Guid Id)
        {
            var request = new GetCommentByIdRequestViewModel { Id = Id };
            var validationResult = await ValidateAsync(request);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<GetCommentByIdResponseViewModel>.Failure(validationResult.ErrorCode, validationResult.Message);
            }

            var result = await Mediator.Send(new GetCommentByIdQuery(Id));
            if (!result.IsSuccess)
            {
                return EndpointResponse<GetCommentByIdResponseViewModel>.Failure(result.ErrorCode, result.Message);
            }

            var response = result.Data.Adapt<GetCommentByIdResponseViewModel>();
            return EndpointResponse<GetCommentByIdResponseViewModel>.Success(response, result.Message);
        }
    }
}


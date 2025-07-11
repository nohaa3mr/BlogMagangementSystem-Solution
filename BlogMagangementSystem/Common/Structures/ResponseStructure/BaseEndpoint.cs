using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Common.Structures.ResponseStructure
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<TRequest> _validator;
        private BaseEndpointParameters<Post> _parameters;

        public IMediator Mediator => _mediator;
        public BaseEndpoint(BaseEndpointParameters<TRequest> parameters )
        {
            _mediator = parameters.Mediator;
            _validator = parameters.Validator;
        }

     

        protected Task<EndpointResponse<TResponse>> ValidateAsync(TRequest request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var validateError = string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
                return Task.FromResult(EndpointResponse<TResponse>.Failure(ErrorCode.InvalidInput));
            }
            return Task.FromResult(EndpointResponse<TResponse>.Success(default!));
        }

    }

}

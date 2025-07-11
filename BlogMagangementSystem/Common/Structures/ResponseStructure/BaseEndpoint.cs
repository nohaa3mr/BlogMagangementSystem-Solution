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

        public IMediator Mediator => _mediator;
        public BaseEndpoint(IMediator mediator , IValidator<TRequest> validator )
        {
            this._mediator = mediator;
            this._validator = validator;
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

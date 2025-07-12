using BlogMagangementSystem.Common.GenericRepository;
using MediatR;

namespace BlogMagangementSystem.Common.Structures.RequestStructure
{
    public class BaseRequestParameters
    {
        private readonly IMediator _mediator;

        public IMediator Mediator => _mediator;

        public BaseRequestParameters(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

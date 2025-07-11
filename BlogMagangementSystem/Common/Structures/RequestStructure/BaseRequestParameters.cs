using BlogMagangementSystem.Common.GenericRepository;
using MediatR;

namespace BlogMagangementSystem.Common.Structures.RequestStructure
{
    public class BaseRequestParameters<TRequest>
    {
        private readonly IMediator _mediator;
        private readonly IGenericRepository<TRequest> _repository;

        public IMediator Mediator => _mediator;
        public IGenericRepository<TRequest> Repository => _repository;

        public BaseRequestParameters(IMediator mediator, IGenericRepository<TRequest> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }
    }
}

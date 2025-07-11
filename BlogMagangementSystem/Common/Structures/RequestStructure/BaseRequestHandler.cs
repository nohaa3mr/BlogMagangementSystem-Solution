using BlogMagangementSystem.Common.GenericRepository;
using MediatR;

namespace BlogMagangementSystem.Common.Structures.RequestStructure
{
   
        public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
        {
            protected IMediator _mediator;
            protected IGenericRepository<TRequest> _repository;

        public BaseRequestHandler(BaseRequestParameters<TRequest> parameters, IGenericRepository<TRequest> repository)
        {
                _mediator = parameters.Mediator;
                _repository = repository;
        }

            public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        }
    
}

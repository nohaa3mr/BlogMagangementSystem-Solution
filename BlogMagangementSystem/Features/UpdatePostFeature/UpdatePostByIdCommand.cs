using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using MediatR;

namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public sealed record UpdatePostByIdCommand(UpdatePostDto Dto) :IRequest<RequestResult<UpdatePostDto>>;
    public class UpdatePostByIdCommandHandler : BaseRequestHandler<UpdatePostByIdCommand, RequestResult<UpdatePostDto>>
    {
        private readonly GenericRepository<Post> _repository;

        public UpdatePostByIdCommandHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters)
        {
            _repository = repository;
        }
        public override Task<RequestResult<UpdatePostDto>> Handle(UpdatePostByIdCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


}

using BlogMagangementSystem.Common.Structures.RequestStructure;
using MediatR;

namespace BlogMagangementSystem.Features.PostModule
{
    public sealed record AddPostCommand(AddPostDto Dto) : IRequest<RequestResult<AddPostRequestViewModel>>;
    public class AddPostCommandHandler  : BaseRequestHandler<AddPostCommand , RequestResult<AddPostRequestViewModel>>
    {
       
    }

}

using BlogMagangementSystem.Features.Comments.CreateComment.DTOs;

namespace BlogMagangementSystem.Features.Comments.CreateComment.Commands
{
    public sealed record CreateCommentCommand(CreateCommentRequestDTO  DTO) :IRequest<RequestResult<CreateCommentResponseDTO>>;
  
  
}

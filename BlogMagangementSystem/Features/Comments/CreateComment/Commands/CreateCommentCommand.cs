using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.Comments.CreateComment.DTOs;
using Mapster;

namespace BlogMagangementSystem.Features.Comments.CreateComment.Commands
{
    public sealed record CreateCommentCommand(CreateCommentRequestDTO DTO) : IRequest<RequestResult<CreateCommentResponseDTO>>;

    public class CreateCommentCommandHandler : BaseRequestHandler<CreateCommentCommand, RequestResult<CreateCommentResponseDTO>>
    {
        private readonly GenericRepository<Comment> _commentRepository;
        private readonly GenericRepository<Post> _postRepository;
        private readonly GenericRepository<User> _userRepository;

        public CreateCommentCommandHandler(
            BaseRequestParameters parameters,
            GenericRepository<Comment> commentRepository,
            GenericRepository<Post> postRepository,
            GenericRepository<User> userRepository) : base(parameters)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public override async Task<RequestResult<CreateCommentResponseDTO>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            if (request.DTO is null)
                return RequestResult<CreateCommentResponseDTO>.Failure(ErrorCode.InvalidInput);

            // Validate Post exists
            var post = await _postRepository.GetByIdAsync(request.DTO.PostId);
            if (post is null || post.IsDeleted || !post.IsActive)
                return RequestResult<CreateCommentResponseDTO>.Failure(ErrorCode.DoesNotExist);

            // Validate User exists
            var user = await _userRepository.GetByIdAsync(request.DTO.UserId);
            if (user is null || user.IsDeleted || !user.IsActive)
                return RequestResult<CreateCommentResponseDTO>.Failure(ErrorCode.DoesNotExist);

            try
            {
                var comment = request.DTO.Adapt<Comment>();
                await _commentRepository.AddAsync(comment);
                await _commentRepository.SaveChangesAsync();

                var response = comment.Adapt<CreateCommentResponseDTO>();
                return RequestResult<CreateCommentResponseDTO>.Success(data: response, message: "Comment Created Successfully");
            }
            catch (Exception)
            {
                return RequestResult<CreateCommentResponseDTO>.Failure(ErrorCode.DatabaseError);
            }
        }
    }
}

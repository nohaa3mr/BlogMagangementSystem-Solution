using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.UserFeatures.DeleteUser;

namespace BlogMagangementSystem.Features.UserFeatures.DeleteUser
{
    public sealed record DeleteUserCommand(Guid Id, Guid RequestingUserId, bool IsAdmin) 
        : IRequest<RequestResult<DeleteUserResponseViewModel>>;

    public class DeleteUserCommandHandler : BaseRequestHandler<DeleteUserCommand, RequestResult<DeleteUserResponseViewModel>>
    {
        private readonly GenericRepository<User> _userRepository;

        public DeleteUserCommandHandler(
            BaseRequestParameters parameters,
            GenericRepository<User> userRepository) : base(parameters)
        {
            _userRepository = userRepository;
        }

        public override async Task<RequestResult<DeleteUserResponseViewModel>> Handle(
            DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            
            if (user is null || user.IsDeleted)
                return RequestResult<DeleteUserResponseViewModel>.Failure(ErrorCode.DoesNotExist);

            // Only admin can delete users, or user can delete themselves
            if (user.ID != request.RequestingUserId && !request.IsAdmin)
            {
                return RequestResult<DeleteUserResponseViewModel>.Failure(ErrorCode.NotAuthorized);
            }

            // Prevent admin from deleting themselves
            if (user.ID == request.RequestingUserId && request.IsAdmin)
            {
                return RequestResult<DeleteUserResponseViewModel>.Failure(ErrorCode.InvalidInput );
            }

            try
            {
                await _userRepository.DeleteAsync(user);
                await _userRepository.SaveChangesAsync();
                
                var response = new DeleteUserResponseViewModel
                {
                    Id = request.Id,
                    Message = "User deleted successfully"
                };
                
                return RequestResult<DeleteUserResponseViewModel>.Success(
                    data: response, 
                    message: "User deleted successfully");
            }
            catch (Exception)
            {
                return RequestResult<DeleteUserResponseViewModel>.Failure(ErrorCode.DatabaseError);
            }
        }
    }
}


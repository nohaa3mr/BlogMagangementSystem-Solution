using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.UserFeatures.UpdateUser;
using Mapster;

namespace BlogMagangementSystem.Features.UserFeatures.UpdateUser
{
    public sealed record UpdateUserCommand(Guid Id, UpdateUserRequestViewModel Request, Guid RequestingUserId, bool IsAdmin) 
        : IRequest<RequestResult<UpdateUserResponseViewModel>>;

    public class UpdateUserCommandHandler : BaseRequestHandler<UpdateUserCommand, RequestResult<UpdateUserResponseViewModel>>
    {
        private readonly GenericRepository<User> _userRepository;

        public UpdateUserCommandHandler(
            BaseRequestParameters parameters,
            GenericRepository<User> userRepository) : base(parameters)
        {
            _userRepository = userRepository;
        }

        public override async Task<RequestResult<UpdateUserResponseViewModel>> Handle(
            UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            
            if (user is null || user.IsDeleted || !user.IsActive)
                return RequestResult<UpdateUserResponseViewModel>.Failure(ErrorCode.DoesNotExist);

            // Check if user is updating themselves or is admin
            if (user.ID != request.RequestingUserId && !request.IsAdmin)
            {
                return RequestResult<UpdateUserResponseViewModel>.Failure(ErrorCode.NotAuthorized);
            }

            try
            {
                var modifiedProperties = new List<string>();

                if (!string.IsNullOrWhiteSpace(request.Request.FirstName))
                {
                    user.FirstName = request.Request.FirstName;
                    modifiedProperties.Add(nameof(User.FirstName));
                }

                if (!string.IsNullOrWhiteSpace(request.Request.LastName))
                {
                    user.LastName = request.Request.LastName;
                    modifiedProperties.Add(nameof(User.LastName));
                }

                if (!string.IsNullOrWhiteSpace(request.Request.Address))
                {
                    user.Address = request.Request.Address;
                    modifiedProperties.Add(nameof(User.Address));
                }

                if (request.Request.PhoneNumber.HasValue)
                {
                    user.PhoneNumber = request.Request.PhoneNumber.Value;
                    modifiedProperties.Add(nameof(User.PhoneNumber));
                }

                if (request.Request.DateOfBirth.HasValue)
                {
                    user.DateOfBirth = request.Request.DateOfBirth.Value;
                    modifiedProperties.Add(nameof(User.DateOfBirth));
                }

                user.UpdatedAt = DateTime.UtcNow;
                modifiedProperties.Add(nameof(BaseEntity.UpdatedAt));

                if (modifiedProperties.Count > 1) // More than just UpdatedAt
                {
                    await _userRepository.UpdateInclude(user, modifiedProperties.ToArray());
                }

                var response = user.Adapt<UpdateUserResponseViewModel>();
                return RequestResult<UpdateUserResponseViewModel>.Success(
                    data: response, 
                    message: "User updated successfully");
            }
            catch (Exception)
            {
                return RequestResult<UpdateUserResponseViewModel>.Failure(ErrorCode.DatabaseError);
            }
        }
    }
}


using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.UserFeatures.GetUserById;
using Mapster;

namespace BlogMagangementSystem.Features.UserFeatures.GetUserById
{
    public sealed record GetUserByIdQuery(Guid Id) : IRequest<RequestResult<GetUserByIdResponseViewModel>>;

    public class GetUserByIdQueryHandler : BaseRequestHandler<GetUserByIdQuery, RequestResult<GetUserByIdResponseViewModel>>
    {
        private readonly GenericRepository<User> _userRepository;

        public GetUserByIdQueryHandler(
            BaseRequestParameters parameters,
            GenericRepository<User> userRepository) : base(parameters)
        {
            _userRepository = userRepository;
        }

        public override async Task<RequestResult<GetUserByIdResponseViewModel>> Handle(
            GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            
            if (user is null || user.IsDeleted || !user.IsActive)
                return RequestResult<GetUserByIdResponseViewModel>.Failure(ErrorCode.DoesNotExist);

            var response = new GetUserByIdResponseViewModel
            {
                Id = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                RoleName = user.Role?.Name ?? "User",
                CreatedAt = user.CreatedAt
            };

            return RequestResult<GetUserByIdResponseViewModel>.Success(data: response, message: "User retrieved successfully");
        }
    }
}


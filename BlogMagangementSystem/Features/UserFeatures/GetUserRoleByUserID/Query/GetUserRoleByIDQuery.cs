namespace BlogMagangementSystem.Features.UserFeatures.GetUserRoleByUserID.Query
{
    public sealed record GetUserRoleByIDQuery (string ID) :IRequest<RequestResult<GetUserRoleByIDResponseDTO>>;
    public class GetUserRoleByIDQueryHandler : BaseRequestHandler<GetUserRoleByIDQuery, RequestResult<GetUserRoleByIDResponseDTO>>
    {
        private readonly GenericRepository<User> _userRepository;
        public GetUserRoleByIDQueryHandler(BaseRequestParameters parameters, GenericRepository<User> userRepository) : base(parameters)
        {
            _userRepository = userRepository;
        }
        public override async Task<RequestResult<GetUserRoleByIDResponseDTO>> Handle(GetUserRoleByIDQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ID))
            {
                return RequestResult<GetUserRoleByIDResponseDTO>.Failure(ErrorCode.InvalidInput);
            }
            try
            {
                var user = await _userRepository.GetByCriteriaAsync(u => u.ID.ToString() == request.ID);
                if (user == null)
                {
                    return RequestResult<GetUserRoleByIDResponseDTO>.Failure(ErrorCode.UserNotFound);
                }
                var responseDto = new GetUserRoleByIDResponseDTO
                {
                    RoleName = user.Role.ToString(), // Assuming RoleName is same as Role for simplicity
                    Success = true
                };
                return RequestResult<GetUserRoleByIDResponseDTO>.Success(
                    data: responseDto,
                    message: "User role retrieved successfully."
                );
            }
            catch (Exception)
            {
                return RequestResult<GetUserRoleByIDResponseDTO>.Failure(ErrorCode.DatabaseError);
            }
        }
    }

}

namespace BlogMagangementSystem.Features.UserFeatures.Login
{
    public sealed record UserLoginCommand(UserLoginDTO DTO) : IRequest<RequestResult<UserLoginDTO>>;
    public class UserLoginCommandHandler : BaseRequestHandler<UserLoginCommand, RequestResult<UserLoginDTO>>
    {
        private readonly BaseRequestParameters _parameters;
        private readonly GenericRepository<User> _repository;
       

        public UserLoginCommandHandler(BaseRequestParameters parameters , GenericRepository<User> repository): base(parameters)
        {
            _parameters = parameters;
            _repository = repository;
        }
        public async override Task<RequestResult<UserLoginDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            if(request.DTO is null)
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);
            var user = await _repository.GetByCriteriaAsync(u => u.Email == request.DTO.Email);
            if (user is null)
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);
            // Verify password and username
            if (!_parameters.PasswordHasher.VerifyPassword(request.DTO.Password, user.Password))
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);

            if (!_parameters. UserNameHasher.VerifyUserName(request.DTO.Username, user.Username))
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);
            var ResultDto = new UserDto()
            {
                Email = user.Email,
                Password = user.Password,
                Token = await _parameters.JwtService.GetTokenAsync(user.Username, user.Email, user.Role, user.ID)

            };
            var Data = ResultDto.Adapt<UserLoginDTO>();
         //   BackgroundJob.Enqueue<EmailService>(service => service.SendWelcomeEmail(user.Email));
            return RequestResult<UserLoginDTO>.Success
            (
                data:Data ,
                message: "User Login Successfully !"
            );


        }
    }


    
}

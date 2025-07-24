
namespace BlogMagangementSystem.Features.UserFeatures.Registeration
{
    public sealed record UserRegisterationCommand(UserDto UserDto) : IRequest<RequestResult<UserDto>>;
    public class UserRegisterationCommandHandler : BaseRequestHandler<UserRegisterationCommand, RequestResult<UserDto>>
    {
        private readonly BaseRequestParameters parameters;
        private readonly GenericRepository<User> _repository;

        public UserRegisterationCommandHandler(BaseRequestParameters parameters , GenericRepository<User> repository ) : base(parameters)
        {
            this.parameters = parameters;
            _repository = repository;
        }
        public override async Task<RequestResult<UserDto>> Handle(UserRegisterationCommand request, CancellationToken cancellationToken)
        {
            if(request.UserDto is null) 
            return RequestResult<UserDto>.Failure(ErrorCode.UserAuthenticationFailed);
        
            var userDto = new UserDto
            {
                FirstName = request.UserDto.FirstName,
                LastName = request.UserDto.LastName,
                Username = parameters.UserNameHasher.HashUserName(request.UserDto.Username),
                Email = request.UserDto.Email,
                Password =parameters.PasswordHasher.HashPassword(request.UserDto.Password),
                Address =request.UserDto.Address,
                PhoneNumber = request.UserDto.PhoneNumber,
                Role = request.UserDto.Role,
            };
            var user = userDto.Adapt<User>();
            try
            {
                
                await _repository.AddAsync(user);
                await _repository.SaveChangesAsync();
                userDto = user.Adapt<UserDto>();
                userDto.Token =await parameters.JwtService.GetTokenAsync(user.Username , user.Email, user.Role);
                BackgroundJob.Enqueue<EmailService>(service => service.SendWelcomeEmail(user.Email));
                return RequestResult<UserDto>.Success
                (
                    data: userDto,
                    message: "User Registered Successfully !"
                );

            }
            catch (Exception)
            {
                return RequestResult<UserDto>.Failure(ErrorCode.DatabaseError);
            }
         
        }
    }
} 

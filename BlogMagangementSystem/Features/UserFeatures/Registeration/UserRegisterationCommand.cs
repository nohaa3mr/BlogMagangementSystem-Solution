using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.JWT_Service;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using BlogMagangementSystem.Features.UserFeatures.HashingAlgorithm;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.UserFeatures.Registeration
{
    public sealed record UserRegisterationCommand(UserDto UserDto) : IRequest<RequestResult<UserDto>>;
    public class UserRegisterationCommandHandler : BaseRequestHandler<UserRegisterationCommand, RequestResult<UserDto>>
    {
        private readonly GenericRepository<User> _repository;
        private readonly JWTService _jwtService;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserNameHaser _usernameHasher;

        public UserRegisterationCommandHandler(BaseRequestParameters parameters , GenericRepository<User> repository , JWTService JwtService , PasswordHasher passwordHasher , UserNameHaser usernameHasher) : base(parameters)
        {
           _repository = repository;
            _jwtService = JwtService;
            _passwordHasher = passwordHasher;
            _usernameHasher = usernameHasher;
        }
        public override async Task<RequestResult<UserDto>> Handle(UserRegisterationCommand request, CancellationToken cancellationToken)
        {
            if(request.UserDto is null) 
            return RequestResult<UserDto>.Failure(ErrorCode.UserAuthenticationFailed);
        
            var userDto = new UserDto
            {
                FirstName = request.UserDto.FirstName,
                LastName = request.UserDto.LastName,
                Username = _usernameHasher.HashUserName(request.UserDto.Username),
                Email = request.UserDto.Email,
                Password =_passwordHasher.HashPassword(request.UserDto.Password),
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
                userDto.Token =await _jwtService.GetTokenAsync(user.Username , user.Email, user.Role);
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

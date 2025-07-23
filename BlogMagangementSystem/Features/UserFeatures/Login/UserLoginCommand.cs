using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.JWT_Service;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.CommonDTOs;
using BlogMagangementSystem.Features.UserFeatures.HashingAlgorithm;
using Mapster;
using MediatR;

namespace BlogMagangementSystem.Features.UserFeatures.Login
{
    public sealed record UserLoginCommand(UserLoginDTO DTO) : IRequest<RequestResult<UserLoginDTO>>;
    public class UserLoginCommandHandler : BaseRequestHandler<UserLoginCommand, RequestResult<UserLoginDTO>>
    {
        private readonly GenericRepository<User> _repository;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserNameHaser _usernameHasher;
        private readonly JWTService _jwtService;

        public UserLoginCommandHandler(BaseRequestParameters parameters , GenericRepository<User> repository , PasswordHasher passwordHasher
            , UserNameHaser usernameHasher , JWTService jwtService): base(parameters)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _usernameHasher = usernameHasher;
            _jwtService = jwtService;
        }
        public async override Task<RequestResult<UserLoginDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            if(request.DTO is null)
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);
            var user = await _repository.GetByCriteriaAsync(u => u.Email == request.DTO.Email);
            if (user is null)
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);
            // Verify password and username
            if (!_passwordHasher.VerifyPassword(request.DTO.Password, user.Password))
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);

            if (!_usernameHasher.VerifyUserName(request.DTO.Username, user.Username))
                return RequestResult<UserLoginDTO>.Failure(ErrorCode.UserAuthenticationFailed);
            var ResultDto = new UserDto()
            {
                Email = user.Email,
                Password = user.Password,
                Token = await _jwtService.GetTokenAsync(user.Username, user.Email, user.Role)
            };
            var Data = ResultDto.Adapt<UserLoginDTO>();
            return RequestResult<UserLoginDTO>.Success
            (
                data:Data ,
                message: "User Login Successfully !"
            );
        }
    }


    
}

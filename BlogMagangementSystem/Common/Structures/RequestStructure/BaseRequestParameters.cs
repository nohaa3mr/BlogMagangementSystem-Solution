using DotNetCore.CAP;

namespace BlogMagangementSystem.Common.Structures.RequestStructure
{
    public class BaseRequestParameters
    {
        private readonly IMediator _mediator;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserNameHasher _userNameHasher;
        private readonly JWTService _jwtService;
        private readonly ICapPublisher _capPublisher;

        public IMediator Mediator => _mediator;
        public PasswordHasher PasswordHasher => _passwordHasher;
        public UserNameHasher UserNameHasher => _userNameHasher;
        public JWTService JwtService => _jwtService;
        public ICapPublisher CapPublisher => _capPublisher;


        public BaseRequestParameters(IMediator mediator , PasswordHasher passwordHasher , UserNameHasher userNameHasher , JWTService jwtService , ICapPublisher capPublisher)
        {
            _mediator = mediator;
            _passwordHasher = passwordHasher;
            _userNameHasher = userNameHasher;
            _jwtService = jwtService;
            _capPublisher = capPublisher;
        }
        public BaseRequestParameters(UserNameHasher userNameHasher , PasswordHasher passwordHasher , JWTService jwtService)
        {
            _userNameHasher = userNameHasher;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }
        public BaseRequestParameters()
        {
            
        }
    }
}

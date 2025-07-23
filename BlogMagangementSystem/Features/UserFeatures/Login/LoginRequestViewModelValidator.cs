using FluentValidation;

namespace BlogMagangementSystem.Features.UserFeatures.Login
{
    public class LoginRequestViewModelValidator : AbstractValidator<UserLoginRequestViewModel>
    {
        public LoginRequestViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        }
    }
    
}

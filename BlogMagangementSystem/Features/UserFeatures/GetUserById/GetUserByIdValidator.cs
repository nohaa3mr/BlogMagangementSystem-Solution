using FluentValidation;

namespace BlogMagangementSystem.Features.UserFeatures.GetUserById
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequestViewModel>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required")
                .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty");
        }
    }
}


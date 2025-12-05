using FluentValidation;

namespace BlogMagangementSystem.Features.UserFeatures.DeleteUser
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserRequestViewModel>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required")
                .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty");
        }
    }
}


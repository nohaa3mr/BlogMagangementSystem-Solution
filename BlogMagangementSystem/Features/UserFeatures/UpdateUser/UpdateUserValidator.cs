using FluentValidation;

namespace BlogMagangementSystem.Features.UserFeatures.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestViewModel>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required")
                .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty");

            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");
            });

            When(x => x.DateOfBirth.HasValue, () =>
            {
                RuleFor(x => x.DateOfBirth.Value)
                    .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past")
                    .GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Date of birth is invalid");
            });
        }
    }
}


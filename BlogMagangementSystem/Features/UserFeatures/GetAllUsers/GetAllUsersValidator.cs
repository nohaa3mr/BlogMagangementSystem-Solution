using FluentValidation;

namespace BlogMagangementSystem.Features.UserFeatures.GetAllUsers
{
    public class GetAllUsersValidator : AbstractValidator<GetAllUsersRequestViewModel>
    {
        public GetAllUsersValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");
        }
    }
}


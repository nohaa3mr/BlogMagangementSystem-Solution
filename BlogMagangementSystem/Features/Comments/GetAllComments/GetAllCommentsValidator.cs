using FluentValidation;

namespace BlogMagangementSystem.Features.Comments.GetAllComments
{
    public class GetAllCommentsValidator : AbstractValidator<GetAllCommentsRequestViewModel>
    {
        public GetAllCommentsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");
        }
    }
}


using FluentValidation;

namespace BlogMagangementSystem.Features.Comments.GetCommentById
{
    public class GetCommentByIdValidator : AbstractValidator<GetCommentByIdRequestViewModel>
    {
        public GetCommentByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Comment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Comment ID cannot be empty");
        }
    }
}


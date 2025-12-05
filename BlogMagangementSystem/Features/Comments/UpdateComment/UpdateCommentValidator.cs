using FluentValidation;

namespace BlogMagangementSystem.Features.Comments.UpdateComment
{
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentRequestViewModel>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Comment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Comment ID cannot be empty");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(2000).WithMessage("Content cannot exceed 2000 characters");
        }
    }
}


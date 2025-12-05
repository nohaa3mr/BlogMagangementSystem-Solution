using FluentValidation;

namespace BlogMagangementSystem.Features.Comments.CreateComment
{
    public class CreateCommentValidator : AbstractValidator<ViewModels.CreateCommentRequestViewModel>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(2000).WithMessage("Content cannot exceed 2000 characters");

            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post ID is required")
                .NotEqual(Guid.Empty).WithMessage("Post ID cannot be empty");
        }
    }
}


using FluentValidation;

namespace BlogMagangementSystem.Features.Comments.DeleteComment
{
    public class DeleteCommentValidator : AbstractValidator<DeleteCommentRequestViewModel>
    {
        public DeleteCommentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Comment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Comment ID cannot be empty");
        }
    }
}


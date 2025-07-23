using BlogMagangementSystem.Features.PostFeatures.UpdatePostFeature;
using FluentValidation;

namespace BlogMagangementSystem.Common.Validators.PostValidation
{
    public class UpdatePostRequestVMValidator : AbstractValidator<UpdatePostRequestViewModel>
    {
        public UpdatePostRequestVMValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required");
        }
    }
    
}

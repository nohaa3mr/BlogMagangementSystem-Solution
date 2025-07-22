using BlogMagangementSystem.Common.Enums;
using BlogMagangementSystem.Features.PostModule;
using FluentValidation;

namespace BlogMagangementSystem.Features.AddPostFeature.Validators.PostValidation
{
    public class AddPostRequestVMValidator : AbstractValidator<AddPostRequestViewModel>
    {
        public AddPostRequestVMValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters long.");
           
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");
            RuleFor(x=>x.Message).Length(0, 500).WithMessage("Message cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Message)); // Optional field, so only validate if provided
        }
    }
}

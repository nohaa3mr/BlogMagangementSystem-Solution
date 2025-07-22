using BlogMagangementSystem.Features.GetPostByIdFeature;
using FluentValidation;

namespace BlogMagangementSystem.Features.AddPostFeature.Validators.PostValidation
{
    public class GetPostByIdValidator : AbstractValidator<GetPostByIdRequestViewModel>
    {
        public GetPostByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Post Id cannot be empty.")
                .GreaterThan(0).WithMessage("Post Id must be greater than zero.");
        }
    }
}

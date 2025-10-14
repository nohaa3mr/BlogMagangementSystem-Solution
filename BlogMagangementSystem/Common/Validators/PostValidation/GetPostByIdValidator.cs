using BlogMagangementSystem.Features.PostFeatures.GetPostByIdFeature;
using FluentValidation;

namespace BlogMagangementSystem.Common.Validators.PostValidation
{
    public class GetPostByIdValidator : AbstractValidator<GetPostByIdRequestViewModel>
    {
        public GetPostByIdValidator()
        {
            RuleFor(x => x.ID).NotEmpty().WithMessage("Post ID must not be empty");
        }
    }
}

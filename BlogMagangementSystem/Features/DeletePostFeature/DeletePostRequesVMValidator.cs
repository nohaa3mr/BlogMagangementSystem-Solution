using FluentValidation;

namespace BlogMagangementSystem.Features.DeletePostFeature
{
    public class DeletePostRequesVMValidator : AbstractValidator<DeletePostRequestViewModel>
    {
        public DeletePostRequesVMValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.")
                .GreaterThan(0).WithMessage("PostId must be greater than 0.");
        }
    }
}

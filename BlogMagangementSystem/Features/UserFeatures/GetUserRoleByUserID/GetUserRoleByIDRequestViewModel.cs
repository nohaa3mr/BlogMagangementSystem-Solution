namespace BlogMagangementSystem.Features.UserFeatures.GetUserRoleByUserID
{
    public record GetUserRoleByIDRequestViewModel(string ID);
    public class GetUserRoleByIDRequestViewModelValidator : AbstractValidator<GetUserRoleByIDRequestViewModel>
    {
        public GetUserRoleByIDRequestViewModelValidator()
        {
            RuleFor(x => x.ID).NotEmpty().WithMessage("User ID must not be empty.");
        }
    }
}

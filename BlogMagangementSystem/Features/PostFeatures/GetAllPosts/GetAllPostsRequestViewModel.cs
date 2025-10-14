namespace BlogMagangementSystem.Features.PostFeatures.GetAllPosts
{
    public sealed record GetAllPostsRequestViewModel(int? PageIndex = 1, int? PageSize = 10 , bool ?SortAsc = false , string?SearchTerm = null!);
    public class GetAllPostsRequestValidator : AbstractValidator<GetAllPostsRequestViewModel>
    {
        public GetAllPostsRequestValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).WithMessage("PageIndex must be greater than 0");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
            RuleFor(x => x.SearchTerm).MaximumLength(100).WithMessage("SearchTerm must be less than 100 characters");
        }
    }

}
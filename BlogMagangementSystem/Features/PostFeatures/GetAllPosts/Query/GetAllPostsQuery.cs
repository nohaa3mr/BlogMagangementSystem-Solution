
namespace BlogMagangementSystem.Features.PostFeatures.GetAllPosts.Query;
public sealed record GetAllPostsQuery(int? PageIndex = 1,
    int? PageSize = 10,
    bool? SortAsc = false,
    string? SearchTerm = null!) : IRequest<RequestResult<IEnumerable<PostDTO>>>;
public class GetAllPostsQueryHandler : BaseRequestHandler<GetAllPostsQuery, RequestResult<IEnumerable<PostDTO>>>
{
    private readonly GenericRepository<Post> _repository;

    public GetAllPostsQueryHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<IEnumerable<PostDTO>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var query = await _repository.GetAllAsync();
        var posts = ApplyFilters(query, request);
        if (posts is null || !posts.Any())
            return RequestResult<IEnumerable<PostDTO>>.Failure(ErrorCode.PostsNotFound);
        var response = posts.Adapt<IEnumerable<PostDTO>>();
        return RequestResult<IEnumerable<PostDTO>>.Success(data: response, message: "Posts Retrieved Successfully");
    }

    private IQueryable<Post> ApplyFilters(IEnumerable<Post> query, GetAllPostsQuery request)
    {
        var filteredQuery = query.AsQueryable();
        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filteredQuery = filteredQuery.Where(p =>
                p.Title.ToLower().Contains(searchTerm) ||
                p.Content.ToLower().Contains(searchTerm));
        }
        // Apply sorting
        filteredQuery = request.SortAsc == true
            ? filteredQuery.OrderBy(p => p.CreatedAt)
            : filteredQuery.OrderByDescending(p => p.CreatedAt);
        // Apply pagination
        if (request.PageIndex.HasValue && request.PageSize.HasValue)
        {
            var skip = (request.PageIndex.Value - 1) * request.PageSize.Value;
            filteredQuery = filteredQuery.Skip(skip).Take(request.PageSize.Value);
        }
        return filteredQuery;
    }
}


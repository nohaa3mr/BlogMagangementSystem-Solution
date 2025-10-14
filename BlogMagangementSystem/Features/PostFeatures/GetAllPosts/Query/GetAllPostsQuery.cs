using BlogMagangementSystem.Common.Helpers;

namespace BlogMagangementSystem.Features.PostFeatures.GetAllPosts.Query;
public sealed record GetAllPostsQuery(int? PageIndex = 1,
    int? PageSize = 10,
    bool? SortAsc = false,
    string? SearchTerm = null!) : IRequest<RequestResult<PagingHelper<PostDTO>>>;
public class GetAllPostsQueryHandler : BaseRequestHandler<GetAllPostsQuery, RequestResult<PagingHelper<PostDTO>>>
{
    private readonly GenericRepository<Post> _repository;

    public GetAllPostsQueryHandler(BaseRequestParameters parameters , GenericRepository<Post> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<PagingHelper<PostDTO>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var query = await _repository.GetAllAsync();
        var posts = ApplyFilters(query, request);
        if (posts is null || !posts.Any())
            return RequestResult<PagingHelper<PostDTO>>.Failure(ErrorCode.PostsNotFound);
        var response = posts.Adapt<PagingHelper<PostDTO>>();

        return RequestResult<PagingHelper<PostDTO>>.Success( response, message: "Posts Retrieved Successfully");
    }

    private IQueryable<Post> ApplyFilters(IQueryable<Post> query, GetAllPostsQuery request)
    {
        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(p =>
                p.Title.ToLower().Contains(searchTerm) ||
                p.Content.ToLower().Contains(searchTerm));
        }
        // Apply sorting
        query = request.SortAsc == true
            ?   query.OrderBy(p => p.CreatedAt)
            : query.OrderByDescending(p => p.CreatedAt);
        // Apply pagination
        if (request.PageIndex.HasValue && request.PageSize.HasValue)
        {
            var skip = (request.PageIndex.Value - 1) * request.PageSize.Value;
                query = query.Skip(skip).Take(request.PageSize.Value);
        }
        return query;
    }
}


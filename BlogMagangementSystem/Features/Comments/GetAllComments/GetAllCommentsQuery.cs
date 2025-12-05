using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Helpers;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.Comments.GetAllComments;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BlogMagangementSystem.Features.Comments.GetAllComments
{
    public sealed record GetAllCommentsQuery(int PageIndex, int PageSize, bool SortAsc, Guid? PostId) 
        : IRequest<RequestResult<PagingHelper<GetAllCommentsResponseViewModel>>>;

    public class GetAllCommentsQueryHandler : BaseRequestHandler<GetAllCommentsQuery, RequestResult<PagingHelper<GetAllCommentsResponseViewModel>>>
    {
        private readonly GenericRepository<Comment> _commentRepository;

        public GetAllCommentsQueryHandler(
            BaseRequestParameters parameters,
            GenericRepository<Comment> commentRepository) : base(parameters)
        {
            _commentRepository = commentRepository;
        }

        public override async Task<RequestResult<PagingHelper<GetAllCommentsResponseViewModel>>> Handle(
            GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = await _commentRepository.GetAllAsync();
            
            // Filter by PostId if provided
            if (request.PostId.HasValue)
            {
                query = query.Where(c => c.PostId == request.PostId.Value);
            }

            // Filter active and non-deleted
            query = query.Where(c => c.IsActive && !c.IsDeleted);

            // Sort
            query = request.SortAsc 
                ? query.OrderBy(c => c.CreatedAt) 
                : query.OrderByDescending(c => c.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var comments = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var responseData = comments.Adapt<List<GetAllCommentsResponseViewModel>>();
            var pagingResult = new PagingHelper<GetAllCommentsResponseViewModel>
            {
                Items = responseData,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Records = totalCount,
                Pages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };

            return RequestResult<PagingHelper<GetAllCommentsResponseViewModel>>.Success(
                data: pagingResult, 
                message: "Comments retrieved successfully");
        }
    }
}


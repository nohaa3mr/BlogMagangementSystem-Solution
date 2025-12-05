using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.GenericRepository;
using BlogMagangementSystem.Common.Helpers;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Features.UserFeatures.GetAllUsers;
using Microsoft.EntityFrameworkCore;

namespace BlogMagangementSystem.Features.UserFeatures.GetAllUsers
{
    public sealed record GetAllUsersQuery(int PageIndex, int PageSize, bool SortAsc, string? SearchTerm) 
        : IRequest<RequestResult<PagingHelper<GetAllUsersResponseViewModel>>>;

    public class GetAllUsersQueryHandler : BaseRequestHandler<GetAllUsersQuery, RequestResult<PagingHelper<GetAllUsersResponseViewModel>>>
    {
        private readonly GenericRepository<User> _userRepository;

        public GetAllUsersQueryHandler(
            BaseRequestParameters parameters,
            GenericRepository<User> userRepository) : base(parameters)
        {
            _userRepository = userRepository;
        }

        public override async Task<RequestResult<PagingHelper<GetAllUsersResponseViewModel>>> Handle(
            GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = await _userRepository.GetAllAsync();
            
            // Filter active and non-deleted
            query = query.Where(u => u.IsActive && !u.IsDeleted);

            // Search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchLower = request.SearchTerm.ToLower();
                query = query.Where(u => 
                    u.FirstName.ToLower().Contains(searchLower) ||
                    u.LastName.ToLower().Contains(searchLower) ||
                    u.Email.ToLower().Contains(searchLower) ||
                    u.Username.ToLower().Contains(searchLower));
            }

            // Sort
            query = request.SortAsc 
                ? query.OrderBy(u => u.CreatedAt) 
                : query.OrderByDescending(u => u.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var users = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var responseData = users.Select(u => new GetAllUsersResponseViewModel
            {
                Id = u.ID,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Username = u.Username,
                RoleName = u.Role?.Name ?? "User",
                CreatedAt = u.CreatedAt
            }).ToList();

            var pagingResult = new PagingHelper<GetAllUsersResponseViewModel>
            {
                Items = responseData,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Records = totalCount,
                Pages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };

            return RequestResult<PagingHelper<GetAllUsersResponseViewModel>>.Success(
                data: pagingResult, 
                message: "Users retrieved successfully");
        }
    }
}


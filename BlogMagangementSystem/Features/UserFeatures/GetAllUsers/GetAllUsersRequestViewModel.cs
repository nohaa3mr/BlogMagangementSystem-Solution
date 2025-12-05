namespace BlogMagangementSystem.Features.UserFeatures.GetAllUsers
{
    public class GetAllUsersRequestViewModel
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool SortAsc { get; set; } = true;
        public string? SearchTerm { get; set; }
    }
}


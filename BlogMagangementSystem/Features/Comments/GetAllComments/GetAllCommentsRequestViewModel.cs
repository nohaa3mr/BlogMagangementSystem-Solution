namespace BlogMagangementSystem.Features.Comments.GetAllComments
{
    public class GetAllCommentsRequestViewModel
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool SortAsc { get; set; } = true;
        public Guid? PostId { get; set; }
    }
}


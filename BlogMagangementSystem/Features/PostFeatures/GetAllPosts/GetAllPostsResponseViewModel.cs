namespace BlogMagangementSystem.Features.PostFeatures.GetAllPosts
{
    public class GetAllPostsResponseViewModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<string> Tags { get; set; } = new HashSet<string>();
        public PostStatus Status { get; set; } = PostStatus.Draft;
    }
}
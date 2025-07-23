namespace BlogMagangementSystem.Features.PostFeatures.DeletePostFeature
{
    public class DeletePostResponseViewModel
    {
        public int PostId { get; set; }
        public string Message { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }

    }
}

namespace BlogMagangementSystem.Common.Enums
{
    [Flags]
    public enum Features
    {
        Login =0 , 
        Register = 1,
        CreatePost = 2,
        UpdatePost = 3,
        DeletePost = 4,
        GetPostById = 5,
        GetPostByName = 6,
        GetAllPosts = 7,

    }
}

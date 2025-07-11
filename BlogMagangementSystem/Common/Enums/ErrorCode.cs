using System.ComponentModel;

namespace BlogMagangementSystem.Common.Enums
{
    public enum ErrorCode
    {
    // General errors (0-99)
    [Description("No error")]
        None = 0,

        [Description("An unknown error occurred")]
        Unknown = 1,

        [Description("The requested resource does not exist")]
        DoesNotExist = 2,

        [Description("Invalid input provided")]
        InvalidInput = 3,

        [Description("You are not authorized to perform this action")]
        NotAuthorized = 4,

        [Description("A server error occurred")]
        ServerError = 5,

        [Description("A database error occurred")]
        DatabaseError = 6,

        [Description("Validation failed")]
        ValidationFailed = 7,

        // Post errors (100-199)
        [Description("Post not found")]
        PostNotFound = 100,

        [Description("Post already exists")]
        PostAlreadyExists = 101,

        [Description("Failed to create post")]
        FailedCreatePost = 102,

        [Description("Failed to update post")]
        FailedUpdatePost = 103,

        [Description("Failed to delete post")]
        FailedDeletePost = 104,

        [Description("Access to post denied")]
        PostAccessDenied = 105,

        [Description("Invalid post data")]
        InvalidPostData = 106,

        // Comment errors (200-299)
        [Description("Comment not found")]
        CommentNotFound = 200,

        [Description("Comment already exists")]
        CommentAlreadyExists = 201,

        [Description("Failed to create comment")]
        FailedCreateComment = 202,

        [Description("Failed to update comment")]
        FailedUpdateComment = 203,

        [Description("Failed to delete comment")]
        FailedDeleteComment = 204,

        [Description("Access to comment denied")]
        CommentAccessDenied = 205,

        [Description("Invalid comment data")]
        InvalidCommentData = 206,

        // Category errors (300-399)
        [Description("Category not found")]
        CategoryNotFound = 300,

        [Description("Category already exists")]
        CategoryAlreadyExists = 301,

        [Description("Failed to create category")]
        FailedCreateCategory = 302,

        [Description("Failed to update category")]
        FailedUpdateCategory = 303,

        [Description("Failed to delete category")]
        FailedDeleteCategory = 304,

        [Description("Access to category denied")]
        CategoryAccessDenied = 305,

        [Description("Invalid category data")]
        InvalidCategoryData = 306,

        // User errors (400-499)
        [Description("User not found")]
        UserNotFound = 400,

        [Description("User already exists")]
        UserAlreadyExists = 401,

        [Description("Failed to create user")]
        FailedCreateUser = 402,

        [Description("Failed to update user")]
        FailedUpdateUser = 403,

        [Description("Failed to delete user")]
        FailedDeleteUser = 404,

        [Description("Access to user denied")]
        UserAccessDenied = 405,

        [Description("Invalid user data")]
        InvalidUserData = 406,

        [Description("User authentication failed")]
        UserAuthenticationFailed = 407,

        // Integration errors (600-699)
        [Description("Integration failed")]
        IntegrationFailed = 600,

        [Description("API synchronization failed")]
        ApiSyncFailed = 601,

        // External service errors (2000-2099)
        [Description("External service unavailable")]
        ExternalServiceUnavailable = 2000,

        [Description("API call limit exceeded")]
        ApiCallLimitExceeded = 2001
    
}
}

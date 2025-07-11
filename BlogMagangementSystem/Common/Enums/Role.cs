namespace BlogMagangementSystem.Common.Enums
{
    public enum Role
    {
        Admin,       // Full access to all features
        User,        // Regular user with limited access
        Moderator,   // Can manage posts and comments but not users
        Guest,      // Can view content but cannot post or comment


    }
}

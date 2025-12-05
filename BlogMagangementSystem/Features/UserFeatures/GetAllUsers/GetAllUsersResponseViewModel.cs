namespace BlogMagangementSystem.Features.UserFeatures.GetAllUsers
{
    public class GetAllUsersResponseViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


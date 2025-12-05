namespace BlogMagangementSystem.Features.UserFeatures.GetUserById
{
    public class GetUserByIdResponseViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


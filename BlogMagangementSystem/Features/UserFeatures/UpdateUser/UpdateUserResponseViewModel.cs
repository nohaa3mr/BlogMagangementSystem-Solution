namespace BlogMagangementSystem.Features.UserFeatures.UpdateUser
{
    public class UpdateUserResponseViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}


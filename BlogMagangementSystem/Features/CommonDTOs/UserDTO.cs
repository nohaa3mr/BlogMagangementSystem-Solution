using BlogMagangementSystem.Common.Enums;

namespace BlogMagangementSystem.Features.CommonDTOs
{
    public class UserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get ; set; }
        public string Password { get; set; }

        public string? Token { get; set; }
        public Role Role { get; set; }

        public string Address { get; set; }
    }
}

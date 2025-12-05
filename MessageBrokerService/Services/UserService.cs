using MessageBrokerService.Models;

namespace MessageBrokerService.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = new();
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public Task<List<User>> GetAllAsync()
        {
            return Task.FromResult(_users.ToList());
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<User> CreateAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            _users.Add(user);
            _logger.LogInformation("User created: {UserId}", user.Id);
            return Task.FromResult(user);
        }

        public Task<User?> UpdateAsync(Guid id, User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing == null) return Task.FromResult<User?>(null);

            existing.Email = user.Email;
            existing.Username = user.Username;
            existing.FullName = user.FullName;
            existing.Bio = user.Bio;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("User updated: {UserId}", id);
            return Task.FromResult<User?>(existing);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Task.FromResult(false);

            _users.Remove(user);
            _logger.LogInformation("User deleted: {UserId}", id);
            return Task.FromResult(true);
        }

        public Task<bool> VerifyEmailAsync(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return Task.FromResult(false);

            user.EmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            _logger.LogInformation("Email verified for user: {UserId}", userId);
            return Task.FromResult(true);
        }
    }
}


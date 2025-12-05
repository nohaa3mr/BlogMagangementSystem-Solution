using MessageBrokerService.IBus;
using MessageBrokerService.UseCases.Events;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerService.UseCases.Controllers
{
    /// <summary>
    /// Example Controller: Demonstrates publishing events from API endpoints
    /// Use Case 1: Publishing events after business operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<UserController> _logger;

        public UserController(IEventBus eventBus, ILogger<UserController> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user and publish UserRegisteredEvent
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            // 1. Perform business logic (save user to database, etc.)
            var userId = Guid.NewGuid();
            _logger.LogInformation("User registered: {UserId}", userId);

            // 2. Publish event after successful registration
            var @event = new UserRegisteredEvent
            {
                UserId = userId,
                Email = request.Email,
                Username = request.Username,
                FullName = request.FullName
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published UserRegisteredEvent for user: {UserId}", userId);

            return Ok(new { UserId = userId, Message = "User registered successfully" });
        }

        /// <summary>
        /// Verify user email and publish UserEmailVerifiedEvent
        /// </summary>
        [HttpPost("verify-email/{userId}")]
        public async Task<IActionResult> VerifyEmail(Guid userId, [FromBody] VerifyEmailRequest request)
        {
            // 1. Verify email logic (check token, update database, etc.)
            _logger.LogInformation("Email verified for user: {UserId}", userId);

            // 2. Publish event
            var @event = new UserEmailVerifiedEvent
            {
                UserId = userId,
                Email = request.Email,
                VerifiedAt = DateTime.UtcNow
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published UserEmailVerifiedEvent for user: {UserId}", userId);

            return Ok(new { Message = "Email verified successfully" });
        }

        /// <summary>
        /// Update user profile and publish UserProfileUpdatedEvent
        /// </summary>
        [HttpPut("profile/{userId}")]
        public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UpdateProfileRequest request)
        {
            // 1. Get existing user data
            var previousEmail = "old@example.com"; // In real app, fetch from database

            // 2. Update user profile
            _logger.LogInformation("Profile updated for user: {UserId}", userId);

            // 3. Determine what changed
            var changedFields = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(request.Email) && request.Email != previousEmail)
            {
                changedFields["Email"] = request.Email;
            }
            if (!string.IsNullOrEmpty(request.FullName))
            {
                changedFields["FullName"] = request.FullName;
            }

            // 4. Publish event with change information
            var @event = new UserProfileUpdatedEvent
            {
                UserId = userId,
                Email = request.Email ?? previousEmail,
                PreviousEmail = previousEmail,
                ChangedFields = changedFields
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published UserProfileUpdatedEvent for user: {UserId}", userId);

            return Ok(new { Message = "Profile updated successfully" });
        }
    }

    // Request DTOs
    public class RegisterUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class VerifyEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    public class UpdateProfileRequest
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }
    }
}


using MessageBrokerService.IBus;
using MessageBrokerService.Models;
using MessageBrokerService.Services;
using MessageBrokerService.UseCases.Events;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService, 
            IEventBus eventBus,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            var createdUser = await _userService.CreateAsync(user);
            
            // Publish UserRegisteredEvent
            var @event = new UserRegisteredEvent
            {
                UserId = createdUser.Id,
                Email = createdUser.Email,
                Username = createdUser.Username,
                FullName = createdUser.FullName
            };
            
            await _eventBus.Publish(@event);
            _logger.LogInformation("Published UserRegisteredEvent for user: {UserId}", createdUser.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(Guid id, [FromBody] User user)
        {
            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            var previousEmail = existingUser.Email;
            var updatedUser = await _userService.UpdateAsync(id, user);
            if (updatedUser == null) return NotFound();

            // Publish UserProfileUpdatedEvent
            var changedFields = new Dictionary<string, object>();
            if (user.Email != previousEmail)
            {
                changedFields["Email"] = user.Email;
            }
            if (user.FullName != existingUser.FullName)
            {
                changedFields["FullName"] = user.FullName;
            }
            if (user.Bio != existingUser.Bio)
            {
                changedFields["Bio"] = user.Bio;
            }

            if (changedFields.Count > 0)
            {
                var @event = new UserProfileUpdatedEvent
                {
                    UserId = updatedUser.Id,
                    Email = updatedUser.Email,
                    PreviousEmail = previousEmail,
                    ChangedFields = changedFields
                };

                await _eventBus.Publish(@event);
                _logger.LogInformation("Published UserProfileUpdatedEvent for user: {UserId}", updatedUser.Id);
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result) return NotFound();
            
            return NoContent();
        }

        [HttpPost("{id}/verify-email")]
        public async Task<IActionResult> VerifyEmail(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userService.VerifyEmailAsync(id);
            if (!result) return BadRequest("Email verification failed");

            // Publish UserEmailVerifiedEvent
            var @event = new UserEmailVerifiedEvent
            {
                UserId = user.Id,
                Email = user.Email,
                VerifiedAt = DateTime.UtcNow
            };

            await _eventBus.Publish(@event);
            _logger.LogInformation("Published UserEmailVerifiedEvent for user: {UserId}", user.Id);

            return Ok(new { message = "Email verification event published" });
        }
    }
}


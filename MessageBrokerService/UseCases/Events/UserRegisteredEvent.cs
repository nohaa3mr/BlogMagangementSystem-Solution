using MessageBrokerService.Events;

namespace MessageBrokerService.UseCases.Events
{
    /// <summary>
    /// Event: Published when a new user registers in the system
    /// </summary>
    public class UserRegisteredEvent : Event
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}


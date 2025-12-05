using MessageBrokerService.Events;

namespace MessageBrokerService.UseCases.Events
{
    /// <summary>
    /// Event: Published when a user verifies their email address
    /// </summary>
    public class UserEmailVerifiedEvent : Event
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime VerifiedAt { get; set; }
    }
}


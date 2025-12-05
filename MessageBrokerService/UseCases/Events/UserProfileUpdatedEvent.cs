using MessageBrokerService.Events;

namespace MessageBrokerService.UseCases.Events
{
    /// <summary>
    /// Event: Published when a user updates their profile
    /// </summary>
    public class UserProfileUpdatedEvent : Event
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PreviousEmail { get; set; }
        public Dictionary<string, object> ChangedFields { get; set; } = new();
    }
}


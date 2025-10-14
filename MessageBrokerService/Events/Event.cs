namespace MessageBrokerService.Events
{
    public abstract class Event
    {
        public DateTime DateTime { get; protected set; }
        protected Event()
        {
            DateTime = DateTime.UtcNow;
        }
    }
}

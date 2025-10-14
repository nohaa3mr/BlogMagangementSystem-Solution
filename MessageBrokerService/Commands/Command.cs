using MessageBrokerService.Events;

namespace MessageBrokerService.Commands
{
    public abstract class Command : Message
    {
        public DateTime DateTime { get; protected set; }
        protected Command()
        {
            DateTime = DateTime.UtcNow;
        }
    }

}

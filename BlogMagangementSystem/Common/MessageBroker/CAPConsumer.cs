using DotNetCore.CAP;

namespace BlogMagangementSystem.Common.MessageBroker
{
    public class CAPConsumer : ICapSubscribe
    {
        [CapSubscribe("post.Iscreated")]
        public void HandlePostCreatedAsync(dynamic message)
        {
            string title = message.Title;
            string content = message.Content;
            string messageBroker = message.Broker; 

            Console.WriteLine($"New post created: {title}");
        }
    }
}

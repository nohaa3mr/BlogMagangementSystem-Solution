using DotNetCore.CAP;

namespace BlogMagangementSystem.Common.MessageBroker
{
    public class CAPConsumer : ICapSubscribe
    {
        [CapSubscribe("post.created")]
        public void HandlePostCreatedAsync(dynamic message)
        {
            string title = message.Title;
            string content = message.Content;

            Console.WriteLine($"New post created: {title}");
        }
    }
}

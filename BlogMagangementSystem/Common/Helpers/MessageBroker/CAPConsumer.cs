using DotNetCore.CAP;

namespace BlogMagangementSystem.Common.Helpers.MessageBroker
{
    public class CAPConsumer : ICapSubscribe
    {
        [CapSubscribe("post.Iscreated")]
        public void HandlePostCreatedAsync(dynamic message)
        {
            string title = message.Title;
            string content = message.Content;

            Console.WriteLine($"New post created: {title}");
        }
    }
}

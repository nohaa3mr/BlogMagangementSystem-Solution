using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace BlogMagangementSystem.Features.MessageBrokersFeature
{
    public class AddMessageUsingCAP
    {
        private readonly ICapPublisher _cap;
        private readonly IChannel _channel;

        public AddMessageUsingCAP(ICapPublisher cap , IChannel channel)
        {
            _cap = cap;
            _channel = channel;
        }

        [HttpPost("PublishMeesageUsingCAP")]
        public async Task PublishMessage(string Message)
        {
            var body = Encoding.UTF8.GetBytes(Message);
           await _channel.BasicPublishAsync("newExchange", "Noha", false, body);

            _cap.Publish("cap", new { Id = 1, Name = "Noha" });
        }

    }
}

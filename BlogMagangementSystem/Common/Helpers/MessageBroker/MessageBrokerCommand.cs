using BlogMagangementSystem.Common.Structures.RequestStructure;
using MediatR;
using RabbitMQ.Client;
using System.Text;

namespace BlogMagangementSystem.Common.Helpers.MessageBroker
{
    public sealed record MessageBrokerCommand(string Message) : IRequest<MessageBrokerCommandResponse>;

    public class MessageBrokerCommandHandler : BaseRequestHandler<MessageBrokerCommand, MessageBrokerCommandResponse>
    {
        // private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly IConnection _connection;

        public MessageBrokerCommandHandler(BaseRequestParameters parameters, IChannel channel , IConnection connection) : base(parameters)
        {
            _channel = channel;
            _connection = connection;
        }
        public override async Task<MessageBrokerCommandResponse> Handle(MessageBrokerCommand request, CancellationToken cancellationToken)
        {
            await _channel.ExchangeDeclareAsync("BlogExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            await _channel.QueueDeclareAsync("BlogQueue", durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync("BlogQueue", "BlogExchange", "post.created");

            var body = Encoding.UTF8.GetBytes(request.Message);
            await _channel.BasicPublishAsync("BlogExchange", "post.created", false, body);

            return MessageBrokerCommandResponse.Success(request.Message);
        }

    }

}

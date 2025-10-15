using MediatR;
using MessageBrokerService.Commands;
using MessageBrokerService.Events;
using MessageBrokerService.IBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace MessageBrokerService.RabbitMQBus
{
    public class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;

        public RabbitMQBus(IMediator mediator , IServiceScopeFactory serviceScopeFactory)
        { 
            _mediator = mediator;
           _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();

        }
        public async Task SendCommand<T>(T command) where T : Command
        {
            await _mediator.Send(command);
        }
        public async Task Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName ="localhost" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
             var eventName = @event.GetType().Name;
          await channel.QueueDeclareAsync(eventName, false, false, false, null);
            var message = System.Text.Json.JsonSerializer.Serialize(@event);
            var body = System.Text.Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync("", eventName, body);

        }

        public async Task Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }
            if (_handlers[eventName].Any(s => s == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }
            _handlers[eventName].Add(handlerType);

          await  StartBasicConsume<T>();
        }

        private async Task StartBasicConsume<T>() where T : Event
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var eventName = typeof(T).Name;

            await channel.QueueDeclareAsync(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Consumer_Received;

            await channel.BasicConsumeAsync(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                ex.Data.Add(ex.Message, ex.InnerException);
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
    }
}


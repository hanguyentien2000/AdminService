using EventBusRabbitMqueue.Abstractions;
using EventBusRabbitMqueue.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Core
{
    public class RabbitMQPublisher : IEventPublisher
    {
        private readonly string _hostName;
        private readonly string _exchangeName;
        private readonly string _exchangeType;

        public RabbitMQPublisher(string hostName = "localhost", string exchangeName = "default_exchange", string exchangeType = ExchangeType.Direct)
        {
            _hostName = hostName;
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
        }

        public void Publish<T>(T @event) where T : BaseEvent
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var routingKey = typeof(T).Name;

            channel.ExchangeDeclare(_exchangeName, _exchangeType, durable: true);
            channel.QueueDeclare(routingKey, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(routingKey, _exchangeName, routingKey);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: routingKey,
                basicProperties: props,
                body: body
            );
        }
    }
}

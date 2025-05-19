using EventBusRabbitMqueue.Abstractions;
using EventBusRabbitMqueue.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EventBusRabbitMqueue.Core
{
    //Gồm TTL, DLQ, Retry, Middleware, Logger
    public abstract class RabbitMQConsumerBase<T> : BackgroundService where T : BaseEvent
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQConsumerBase<T>> _logger;
        private readonly IErrorLogger _errorLogger;
        private readonly string _hostName;
        private readonly string _exchangeName;
        private readonly int _ttlMilliseconds;

        protected RabbitMQConsumerBase(IServiceProvider serviceProvider,
                                   ILogger<RabbitMQConsumerBase<T>> logger,
                                   string hostName = "localhost",
                                   string exchangeName = "default_exchange",
                                   int ttlMilliseconds = 60000)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hostName = hostName;
            _exchangeName = exchangeName;
            _ttlMilliseconds = ttlMilliseconds;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var queueName = typeof(T).Name;
            var deadLetterQueue = queueName + ".dlq";

            channel.QueueDeclare(deadLetterQueue, durable: true, exclusive: false, autoDelete: false);

        //    var args = new Dictionary<string, object>
        //{
        //    { "x-dead-letter-exchange", "" },
        //    { "x-dead-letter-routing-key", deadLetterQueue },
        //    { "x-message-ttl", _ttlMilliseconds }
        //};

            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: true);
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, _exchangeName, routingKey: queueName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<T>>();
                var middlewares = scope.ServiceProvider.GetServices<IMessageMiddleware<T>>().ToList();

                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<T>(json);

                    var pipeline = new MessageMiddlewareExecutor<T>(middlewares, (msg) => handler.HandleAsync(msg));
                    await pipeline.ExecuteAsync(message!);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling message");
                    await _errorLogger.LogErrorAsync(queueName, ea.Body.ToArray(), ex);
                    channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}

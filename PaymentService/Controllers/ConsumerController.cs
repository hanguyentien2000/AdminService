
using EventBusRabbitMqueue;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    public class ConsumerController : BackgroundService
    {
        private readonly IRabbitMqueueHandler _rabbitMQService;

        public ConsumerController(IRabbitMqueueHandler rabbitMQService)
        {
            rabbitMQService = _rabbitMQService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queueName = "testQueue";

            _rabbitMQService.StartConsuming(queueName, message =>
            {
                Console.WriteLine($"[x] Received: {message}");
            });

            return Task.CompletedTask;
        }
    }
}

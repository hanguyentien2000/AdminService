using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace EventBusRabbitMqueue
{
    public interface IRabbitMqueueHandler
    {
        void PublishMessage(string message, string queueName);
        void StartConsuming(string queueName, Action<string> onMessageReceived);
    }
}

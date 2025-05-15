using EventBusRabbitMqueue.Core;
using PaymentService.Business.Core;

namespace PaymentService.Intergrate
{
    public class UserCreatedConsumer : RabbitMQConsumerBase<UserCreatedEvent>
    {
        public UserCreatedConsumer(IServiceProvider provider,
                                   ILogger<RabbitMQConsumerBase<UserCreatedEvent>> logger
                                  )
            : base(provider, logger,
                   hostName: "localhost",
                   exchangeName: "user.payment.exchange",
                   ttlMilliseconds: 30000)
        {
        }
    }
}

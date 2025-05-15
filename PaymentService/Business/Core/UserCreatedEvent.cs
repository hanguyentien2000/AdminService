using EventBusRabbitMqueue.Abstractions;

namespace PaymentService.Business.Core
{
    public class UserCreatedHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedHandler> _logger;

        public UserCreatedHandler(ILogger<UserCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UserCreatedEvent @event)
        {
            _logger.LogInformation("PaymentService: Received new user {UserId} - {Email}",
                @event.UserId, @event.Email);

            // Logic thanh toán hoặc tạo tài khoản payment...
            return Task.CompletedTask;
        }
    }
}

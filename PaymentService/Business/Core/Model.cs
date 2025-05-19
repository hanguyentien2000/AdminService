using EventBusRabbitMqueue.Models;

namespace PaymentService.Business.Core
{
    public class UserCreatedEvent : BaseEvent
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
    }
}

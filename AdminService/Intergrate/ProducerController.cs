using EventBusRabbitMqueue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AdminService.Intergrate
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly IRabbitMqueueHandler _rabbitMQService;

        public ProducerController (IRabbitMqueueHandler rabbitMQService)
        {
            rabbitMQService = _rabbitMQService;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            string queueName = "testQueue"; // Tên của queue
            _rabbitMQService.PublishMessage(message, queueName);
            return Ok($"Message sent: {message}");
        }
    }
}

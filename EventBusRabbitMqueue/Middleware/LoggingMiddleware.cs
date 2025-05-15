using EventBusRabbitMqueue.Abstractions;
using EventBusRabbitMqueue.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Middleware
{
    public class LoggingMiddleware<T> : IMessageMiddleware<T> where T : BaseEvent
    {
        private readonly ILogger<LoggingMiddleware<T>> _logger;

        public LoggingMiddleware(ILogger<LoggingMiddleware<T>> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(T message, Func<Task> next)
        {
            _logger.LogInformation("Processing {MessageType} - ID: {Id}", typeof(T).Name, message.Id);
            await next();
            _logger.LogInformation("Processed {MessageType} - ID: {Id}", typeof(T).Name, message.Id);
        }
    }
}

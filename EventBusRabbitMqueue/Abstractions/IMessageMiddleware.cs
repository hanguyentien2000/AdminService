using EventBusRabbitMqueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Abstractions
{
    public interface IMessageMiddleware<T> where T : BaseEvent
    {
        Task InvokeAsync(T message, Func<Task> next);
    }
}

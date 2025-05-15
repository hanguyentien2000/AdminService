using EventBusRabbitMqueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Abstractions
{
    public interface IEventHandler<in T> where T : BaseEvent
    {
        Task HandleAsync(T @event);
    }
}

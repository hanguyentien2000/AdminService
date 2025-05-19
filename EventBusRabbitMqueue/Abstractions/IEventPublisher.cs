using EventBusRabbitMqueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventBusRabbitMqueue.Abstractions
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : BaseEvent;
    }
}

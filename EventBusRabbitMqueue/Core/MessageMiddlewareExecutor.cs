using EventBusRabbitMqueue.Abstractions;
using EventBusRabbitMqueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Core
{
    public class MessageMiddlewareExecutor<T> where T : BaseEvent
    {
        private readonly IEnumerable<IMessageMiddleware<T>> _middlewares;
        private readonly Func<T, Task> _finalHandler;

        public MessageMiddlewareExecutor(IEnumerable<IMessageMiddleware<T>> middlewares, Func<T, Task> finalHandler)
        {
            _middlewares = middlewares;
            _finalHandler = finalHandler;
        }

        public async Task ExecuteAsync(T message)
        {
            var enumerator = _middlewares.Reverse().GetEnumerator();
            Func<Task> next = () => _finalHandler(message);

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var previousNext = next;
                next = () => current.InvokeAsync(message, previousNext);
            }

            await next();
        }
    }
}

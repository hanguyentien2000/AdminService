using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Abstractions
{
    public interface IErrorLogger
    {
        Task LogErrorAsync(string queueName, byte[] rawMessage, Exception ex);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EST.MIT.Payment.Interfaces
{
    public interface IEventQueueService
    {
        Task CreateMessage(string status, string action, string message, string data);
    }
}

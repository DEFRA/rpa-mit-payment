using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Interfaces
{
    public interface IServiceBus
    {
        Task SendServiceBus(string message);
    }
}

using System.Threading.Tasks;

namespace EST.MIT.Payment.Interfaces
{
    public interface IServiceBus
    {
        Task SendServiceBus(string serviceBus);
    }
}

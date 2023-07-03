namespace EST.MIT.Payment.Interfaces
{
    public interface IEventQueueService
    {
        Task CreateMessage(string status, string action, string message, string data);
    }
}

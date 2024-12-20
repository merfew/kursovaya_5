namespace kursovay_transfer.RabbitMQ
{
    public interface ITransferBrokerService
    {
        Task Subscribe(string exchange, Func<string, Task> handler);
    }
}
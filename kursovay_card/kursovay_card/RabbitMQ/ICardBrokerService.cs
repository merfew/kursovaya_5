namespace kursovay_card.RabbitMQ
{
    public interface ICardBrokerService
    {
        Task Subscribe(string exchange, Func<string, Task> handler);
    }
}
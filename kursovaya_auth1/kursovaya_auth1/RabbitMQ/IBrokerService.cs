namespace kursovaya_auth1.RabbitMQ
{
    public interface IBrokerService
    {
        Task SendMessage(string exchange, object message);
    }
}
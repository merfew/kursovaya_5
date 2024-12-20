using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace kursovaya_auth1.RabbitMQ
{
    public class BrokerService: IBrokerService
    {
        private readonly ConnectionFactory _connectionFactory;

        public BrokerService()
        {
            _connectionFactory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task SendMessage(string exchange, object message)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout);
            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            await channel.BasicPublishAsync(exchange, "", body);
            Console.WriteLine("Сообщение отправлено", message);
        }
    }
}

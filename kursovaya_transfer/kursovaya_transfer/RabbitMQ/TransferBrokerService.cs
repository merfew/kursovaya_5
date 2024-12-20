using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace kursovay_transfer.RabbitMQ
{
    public class TransferBrokerService : ITransferBrokerService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IList<AsyncEventingBasicConsumer> _consumers = [];

        public TransferBrokerService()
        {
            _connectionFactory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task Subscribe(string exchange, Func<string, Task> handler)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout);
            QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
            string queueName = queueDeclareResult.QueueName;
            await channel.QueueBindAsync(queueName, exchange, "");
            var consumer = new AsyncEventingBasicConsumer(channel);
            _consumers.Add(consumer);
            Console.WriteLine($"Subscribing to '{exchange}', queue name is '{queueName}'");
            consumer.ReceivedAsync += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received '{message}'");
                await handler(message);
            };
            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
        }
    }
}

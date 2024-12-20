﻿//using RabbitMQ.Client.Events;
//using RabbitMQ.Client;
//using System.Threading.Tasks;
//using System.Threading;
//using Microsoft.Extensions.Hosting;
//using System.Text;
//using System.Diagnostics;
//using System;


//public class RabbitMqListener : BackgroundService
//{
//    private IConnection _connection;
//    private IModel _channel;

//    public RabbitMqListener()
//    {
//        var factory = new ConnectionFactory { HostName = "localhost" };
//        _connection = factory.CreateConnection();
//        _channel = _connection.CreateModel();
//        _channel.QueueDeclare(queue: "MyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
//    }

//    protected override Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        stoppingToken.ThrowIfCancellationRequested();

//        var consumer = new EventingBasicConsumer(_channel);
//        consumer.Received += (ch, ea) =>
//        {
//            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

//            Debug.WriteLine($"Получено сообщение: {content}");

//            _channel.BasicAck(ea.DeliveryTag, false);
//        };

//        _channel.BasicConsume("MyQueue1", false, consumer);

//        return Task.CompletedTask;
//    }

//    public override void Dispose()
//    {
//        _channel.Close();
//        _connection.Close();
//        base.Dispose();
//    }
//}
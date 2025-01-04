using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using kursovay_transfer.RabbitMQ;
using kursovaya_transfer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace kursah_5semestr.Services
{
    public class RabbitMqListener : IDataUpdaterService
    {
        private ITransferBrokerService _brokerService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IUserDataFunc _userData;

        public RabbitMqListener(ITransferBrokerService brokerService, IServiceScopeFactory scopeFactory, IUserDataFunc userData)
        {
            _brokerService = brokerService;
            _scopeFactory = scopeFactory;
            _userData = userData;
        }

        private async Task ProcessMessage(string message)
        {
            try
            {
                var id = await Task.Run(() => JsonSerializer.Deserialize<UserData>(message));
                _userData.SetVariable(id!.user_id.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Start()
        {
            await _brokerService.Subscribe("changes", ProcessMessage);
        }
    }
}

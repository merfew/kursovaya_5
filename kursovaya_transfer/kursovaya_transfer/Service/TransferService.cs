using kursovaya_transfer.Model;
using kursovaya_transfer.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kursovaya_transfer.Service
{
    public class TransferService: ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        public TransferService(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }
        public async Task<List<Transfer>> GetHistory(string id)
        {
            int.TryParse(id, out int Id);
            var history = await _transferRepository.GetHistory(Id);
            return history;
        }

        public async Task<string> TransferInside(Transfer transfer, string User_Id, string sender_Id, string recipient_Id)
        {
            int.TryParse(User_Id, out int user_Id);
            int.TryParse(sender_Id, out int sender_id);
            int.TryParse(recipient_Id, out int recipient_id);

            Card send_card = await _transferRepository.GetCard(sender_id);
            if (send_card == null)
            {
                return "Нет карты отправителя";
            }
            await _transferRepository.NewSendBalance(send_card, transfer.sum);

            Card reci_card = await _transferRepository.GetCard(recipient_id);
            if (reci_card == null) 
            { 
                return "Нет карты получателя"; 
            }
            await _transferRepository.NewReciBalance(reci_card, transfer.sum);

            Transfer newTransfer = new Transfer
            {
                user_id = user_Id,
                sender_id = send_card.card_id,
                recipient_id = reci_card.card_id,
                sum = transfer.sum,
            };

            await _transferRepository.CreateTransfer(newTransfer);
            return ("Перевод выполнен успешно");
        }

        public async Task<string> TransferOutside(Transfer transfer, string User_Id, string sender_Id, string account_num)
        {
            int.TryParse(User_Id, out int user_Id);
            int.TryParse(sender_Id, out int sender_id);
            //int.TryParse(recipient_Id, out int recipient_id);

            Card send_card = await _transferRepository.GetCard(sender_id);
            if (send_card == null)
            {
                return "Нет карты отправителя";
            }
            await _transferRepository.NewSendBalance(send_card, transfer.sum);

            Card reci_card = await _transferRepository.GetCardByAccount(account_num);
            if (reci_card == null)
            {
                return "Нет карты получателя";
            }
            await _transferRepository.NewReciBalance(reci_card, transfer.sum);

            Transfer newTransfer = new Transfer
            {
                user_id = user_Id,
                sender_id = send_card.card_id,
                recipient_id = reci_card.card_id,
                sum = transfer.sum,
            };

            await _transferRepository.CreateTransfer(newTransfer);
            return ("Перевод выполнен успешно");
        }
    }
}

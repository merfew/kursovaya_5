using kursovaya_transfer.Model;
using kursovaya_transfer.Object;
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
        public async Task<List<Transfer>?> GetHistory(string id)
        {
            int.TryParse(id, out int Id);
            var history = await _transferRepository.GetHistory(Id);
            return history;
        }

        public async Task<string> TransferInside(InTransferObj transferObj, string User_Id)
        {
            int.TryParse(User_Id, out int user_Id);

            if (transferObj.sum <= 0)
            {
                return "Сумма перевода не может быть меньше нуля";
            }

            Card? send_card = await _transferRepository.GetCard(transferObj.sender_id);
            if (send_card == null)
            {
                return "Нет карты отправителя";
            }
            string str = await _transferRepository.NewSendBalance(send_card, transferObj.sum);
            if (str == "Недостаточно средств на карте")
            {
                return str;
            }

            Card? reci_card = await _transferRepository.GetCard(transferObj.recipient_id);
            if (reci_card == null) 
            { 
                return "Нет карты получателя"; 
            }
            await _transferRepository.NewReciBalance(reci_card, transferObj.sum);

            Transfer newTransfer = new Transfer
            {
                user_id = user_Id,
                sender_id = send_card.card_id,
                recipient_id = reci_card.card_id,
                sum = transferObj.sum,
            };

            await _transferRepository.CreateTransfer(newTransfer);
            return ("Перевод выполнен успешно");
        }

        public async Task<string> TransferOutside(OutTransferObj transferObj, string User_Id)
        {
            int.TryParse(User_Id, out int user_Id);

            if (transferObj.sum <= 0)
            {
                return "Сумма перевода не может быть меньше нуля";
            }

            Card? send_card = await _transferRepository.GetCard(transferObj.sender_id);
            if (send_card == null)
            {
                return "Нет карты отправителя";
            }
            string str = await _transferRepository.NewSendBalance(send_card, transferObj.sum);
            if (str == "Недостаточно средств на карте")
            {
                return str;
            }

            Card? reci_card = await _transferRepository.GetCardByAccount(transferObj.account_number);
            if (reci_card == null)
            {
                return "Нет карты получателя";
            }
            await _transferRepository.NewReciBalance(reci_card, transferObj.sum);

            Transfer newTransfer = new Transfer
            {
                user_id = user_Id,
                sender_id = send_card.card_id,
                recipient_id = reci_card.card_id,
                sum = transferObj.sum,
            };

            await _transferRepository.CreateTransfer(newTransfer);
            return ("Перевод выполнен успешно");
        }
    }
}

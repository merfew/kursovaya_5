using kursah_5semestr.Services;
using kursovay_card.Model;
using kursovay_card.Object;
using kursovay_card.Repository;
using kursovaya_card;

namespace kursovay_card.Service
{
    public class CardService: ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IDataUpdaterService _dataUpdaterService;
        public CardService(ICardRepository cardRepository, IDataUpdaterService dataUpdaterService)
        {
            _cardRepository = cardRepository;
            _dataUpdaterService = dataUpdaterService;
        }

        public async Task<List<Card>?> GetCards(string Id)
        {
            int.TryParse(Id, out int id);
            var data = await _cardRepository.GetCards(id);
            return (data);
        }

        public async Task<Card?> GetInfoCard(int id)
        {
            Card? card = await _cardRepository.GetInfoCard(id);
            return (card);
        }

        public async Task<string?> CreateCard(CardObj cardObj, string id)
        {
            int.TryParse(id, out int Id);
            if (cardObj.balance < 0)
            {
                return "Баланс карты не может быть меньше нуля";
            }
            Card card = new Card
            {
                user_id = Id,
                name = cardObj.name,
                account_number = cardObj.account_number,
                data = cardObj.data,
                cvc = cardObj.cvc,
                balance = cardObj.balance,

            };
            await _cardRepository.CreateCard(card);
            return null;
        }

        public async Task<UserData> CreateUser(UserData userData)
        {
            return await Task.Run(() => userData);
        }
    }
}

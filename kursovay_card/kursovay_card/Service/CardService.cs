using kursah_5semestr.Services;
using kursovay_card.Model;
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
        //public async Task<List<Card>?> GetCards(string id)
        //{
        //    if (id != null)
        //    {
        //        int.TryParse(id, out int Id);
        //        var data = await _cardRepository.GetCards(Id);
        //        return (data);

        //    }
        //    return null;
        //}

        public async Task<List<Card>?> GetCards(string Id)
        {
            int.TryParse(Id, out int id);
            if (id != null)
            {
                var data = await _cardRepository.GetCards(id);
                return (data);

            }
            return null;
        }

        public async Task<Card?> GetInfoCard(string id)
        {
            if (id != null)
            {
                int.TryParse(id, out int Id);
                var card = await _cardRepository.GetInfoCard(Id);
                return (card);
            }
            return null;
        }

        public async Task<string?> CreateCard(Card card, string id)
        {
            int.TryParse(id, out int Id);
            await _cardRepository.CreateCard(card, Id);
            return null;
        }

        public async Task<UserData> CreateUser(UserData userData)
        {
            return userData;
        }
    }
}

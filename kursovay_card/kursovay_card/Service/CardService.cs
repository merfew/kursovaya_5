using kursovay_card.Model;
using kursovay_card.Repository;

namespace kursovay_card.Service
{
    public class CardService: ICardService
    {
        private readonly ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public async Task<List<Card>?> GetCards(string id)
        {
            if (id != null)
            {
                int.TryParse(id, out int Id);
                var data = await _cardRepository.GetCards(Id);
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
    }
}

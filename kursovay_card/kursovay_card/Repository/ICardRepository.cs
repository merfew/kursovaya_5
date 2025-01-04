using kursovay_card.Model;

namespace kursovay_card.Repository
{
    public interface ICardRepository
    {
        Task<List<Card>> GetCards(int id);
        Task<Card?> GetInfoCard(int id);
        Task<Card> CreateCard(Card card);
    }
}
using kursovay_card.Model;

namespace kursovay_card.Service
{
    public interface ICardService
    {
        Task<List<Card>?> GetCards(string id);
        Task<Card?> GetInfoCard(string id);
        Task<string?> CreateCard(Card card, string id);
    }
}
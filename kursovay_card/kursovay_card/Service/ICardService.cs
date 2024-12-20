using kursovay_card.Model;
using kursovaya_card;

namespace kursovay_card.Service
{
    public interface ICardService
    {
        Task<List<Card>?> GetCards(string id);
        //Task<List<Card>?> GetCards(UserData userData);
        Task<Card?> GetInfoCard(string id);
        Task<string?> CreateCard(Card card, string id);
        Task<UserData> CreateUser(UserData userData);
    }
}
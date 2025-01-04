using kursovay_card.Model;
using kursovay_card.Object;
using kursovaya_card;

namespace kursovay_card.Service
{
    public interface ICardService
    {
        Task<List<Card>?> GetCards(string id);
        Task<Card?> GetInfoCard(int id);
        Task<string?> CreateCard(CardObj cardObj, string id);
        Task<UserData> CreateUser(UserData userData);
    }
}
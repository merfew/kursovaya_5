using kursovaya_transfer.Model;

namespace kursovaya_transfer.Repository
{
    public interface ITransferRepository
    {
        Task<List<Transfer>?> GetHistory(int id);
        Task<Card?> GetCard(int id);
        Task<Card?> GetCardByAccount(string? number);
        Task<Transfer> CreateTransfer(Transfer transfer);
        Task<string> NewSendBalance(Card card, float newBalance);
        Task<string> NewReciBalance(Card card, float newBalance);
    }
}
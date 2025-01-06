using kursovaya_transfer.Model;
using Microsoft.EntityFrameworkCore;

namespace kursovaya_transfer.Repository
{
    public class TransferRepository: ITransferRepository
    {
        private readonly AppDbContext _context;
        public TransferRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Transfer>?> GetHistory(int id)
        {
            var history = await _context.Transfer.AsNoTracking().Where(c => c.user_id == id).ToListAsync();
            return history;
        }

        public async Task<Card?> GetCard(int id)
        {
            Card? card = await _context.Card.FirstOrDefaultAsync(c => c.card_id == id);
            return (card);
        }

        public async Task<Card?> GetCardByAccount(string? number)
        {
            Card? card = await _context.Card.FirstOrDefaultAsync(c => c.account_number == number);
            return (card);
        }

        public async Task<Transfer> CreateTransfer(Transfer transfer)
        {
            await _context.Transfer.AddAsync(new Transfer
            {
                user_id = transfer.user_id,
                sender_id = transfer.sender_id,
                recipient_id = transfer.recipient_id,
                sum = transfer.sum,
            });

            await _context.SaveChangesAsync();

            return transfer;
        }
        public async Task<string> NewSendBalance(Card card, float newBalance)
        {
            if (card.balance - newBalance < 0)
            {
                return "Недостаточно средств на карте";
            }
            card.balance = card.balance - newBalance;
            await _context.SaveChangesAsync();
            return "Баланс обновлен";
        }
        public async Task<string> NewReciBalance(Card card, float newBalance)
        {
            card.balance = card.balance + newBalance;
            await _context.SaveChangesAsync();
            return "Баланс обновлен";
        }
    }
}

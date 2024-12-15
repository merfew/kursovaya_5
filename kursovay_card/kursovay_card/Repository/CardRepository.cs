using kursovay_card.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace kursovay_card.Repository
{
    public class CardRepository: ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Card>> GetCards(int id)
        {
            var data = await _context.Card.AsNoTracking().Where(c => c.user_id == id).ToListAsync();
            return (data);
        }

        public async Task<Card?> GetInfoCard(int id)
        {
            Card? card = await _context.Card.FirstOrDefaultAsync(c => c.card_id == id);
            return (card);
        }

        public async Task<Card> CreateCard(Card card, int id)
        {
            await _context.Card.AddAsync(new Card
            {
                card_id = card.card_id,
                user_id = id,
                name = card.name,
                account_number = card.account_number,
                data = card.data,
                cvc = card.cvc,
                balance = card.balance
            });

            await _context.SaveChangesAsync();

            return card;
        }
    }
}

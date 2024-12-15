using kursovaya_auth1.Model;
using Microsoft.EntityFrameworkCore;

namespace kursovaya_auth1.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Login(string? email, string? password)
        {
            User? user = await _context.User.FirstOrDefaultAsync(c => c.email == email);
            return (user);
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.User.AddAsync(new User
            {
                name = user.name,
                surname = user.surname,
                phone_number = user.phone_number,
                email = user.email,
                password = user.password
            });

            await _context.SaveChangesAsync();

            return user;
        }
    }
}

using kursovaya_auth1.Model;

namespace kursovaya_auth1.Repository
{
    public interface IAuthRepository
    {
        Task<User> CreateUser(User user);
        Task<User> Login(string? email, string? password);
    }
}
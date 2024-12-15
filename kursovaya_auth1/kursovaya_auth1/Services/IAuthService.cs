using kursovaya_auth1.Model;

namespace kursovaya_auth1.Services
{
    public interface IAuthService
    {
        Task<string> CreateUser(User user);
        Task<(int, string)> Login(string? email, string? password);
    }
}
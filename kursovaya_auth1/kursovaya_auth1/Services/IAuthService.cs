using kursovaya_auth1.Model;
using kursovaya_auth1.Object;

namespace kursovaya_auth1.Services
{
    public interface IAuthService
    {
        Task<AnswerObj> CreateUser(UserObj userObj);
        Task<AnswerObj> Login(LoginObj loginObj);
    }
}
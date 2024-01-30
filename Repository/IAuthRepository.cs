using BeritaKuAPI.Model;

namespace BeritaKuAPI.Repository
{
    public interface IAuthRepository
    {
        User Authenticate(string username, string password);
        bool Register(User user);
        bool CheckUserExistByUsername(string username);
    }
}

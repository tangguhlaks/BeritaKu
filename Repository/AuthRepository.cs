using BeritaKuAPI.Data;
using BeritaKuAPI.Model;

namespace BeritaKuAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username,string password)
        {
           
            var user = _context.users.Where(x=> x.Username == username).FirstOrDefault();
            return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user:null;
        }

        public bool CheckUserExistByUsername(string username)
        {
            return _context.users.Where(x => x.Username == username).FirstOrDefault() != null ? true : false;
        }

        public bool Register(User user)
        {
            try
            {
                _context.users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}

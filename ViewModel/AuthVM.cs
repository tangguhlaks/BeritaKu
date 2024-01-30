namespace BeritaKuAPI.ViewModel
{
    public class AuthVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public AuthVM() { }
        public AuthVM(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

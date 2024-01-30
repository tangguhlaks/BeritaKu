using System.ComponentModel.DataAnnotations.Schema;

namespace BeritaKuAPI.Model
{
    [Table("users")]
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isActived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

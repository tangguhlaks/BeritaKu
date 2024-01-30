using System.ComponentModel.DataAnnotations.Schema;

namespace BeritaKuAPI.Model
{
    [Table("beritas")]
    public class Berita
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}

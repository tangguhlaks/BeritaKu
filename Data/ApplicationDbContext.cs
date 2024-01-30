using BeritaKuAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BeritaKuAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        public DbSet<Berita> beritas { get; set; }
    }
}

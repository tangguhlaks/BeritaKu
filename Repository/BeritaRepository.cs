using BeritaKuAPI.Data;
using BeritaKuAPI.Model;
using BeritaKuAPI.ViewModel;

namespace BeritaKuAPI.Repository
{
    public class BeritaRepository : IBeritaRepository
    {
        private readonly ApplicationDbContext _context;

        public BeritaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public string Create(Berita berita)
        {
            try
            {
                _context.beritas.Add(berita);
                _context.SaveChanges();
                return berita.Id.ToString();
              
            }catch (Exception ex)
            {
                return "";
            }
        }

        public bool Delete(string id)
        {
            try
            {
                var berita = _context.beritas.Where(x => x.Id.ToString() == id).First();
                _context.beritas.Remove(berita);
                _context.SaveChanges();

                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public List<BeritaVM> GetAll()
        {
            try
            {
                return _context.beritas.Select(x => new BeritaVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    CreatedBy = x.CreatedBy
                }).ToList();
            }catch (Exception ex)
            {
                return new List<BeritaVM>();
            }
        }

        public List<BeritaVM> GetByCreatedBy(string username)
        {
            try { 
                return _context.beritas.Where(x => x.CreatedBy == username).Select(x => new BeritaVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    CreatedBy = x.CreatedBy
                }).ToList();
            }catch(Exception ex)
            {
                return new List<BeritaVM>();
            }
        }

        public BeritaVM GetById(string id)
        {
            try
            {
                return _context.beritas.Where(x => x.Id.ToString() == id).Select(x => new BeritaVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    CreatedBy = x.CreatedBy
                }).First();
            }catch (Exception ex)
            {
                return new BeritaVM();
            }
        }

        public string Update(Berita berita)
        {
            try
            {
                _context.beritas.Update(berita);
                _context.SaveChanges();
                return berita.Id.ToString();

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}

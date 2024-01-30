using BeritaKuAPI.Model;
using BeritaKuAPI.ViewModel;

namespace BeritaKuAPI.Repository
{
    public interface IBeritaRepository
    {
        string Create(Berita berita);
        string Update(Berita berita);
        bool Delete(string id);
        BeritaVM GetById(string id);
        List<BeritaVM> GetByCreatedBy(string username);
        List<BeritaVM> GetAll();
    }
}

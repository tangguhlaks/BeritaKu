using BeritaKuAPI.ViewModel;

namespace BeritaKuAPI.Response
{
    public class BeritaResponseCreate
    {
        public string Id { get; set; } 

        public BeritaResponseCreate(string id) {
            Id = id;
        }

    }
    public class BeritaResponseGetById
    {
        public BeritaVM Data { get; set; }

        public BeritaResponseGetById(BeritaVM data)
        {
            Data = data;
        }

    }
    public class BeritaResponseGetAll
    {
        public List<BeritaVM> Data { get; set; }

        public BeritaResponseGetAll(List<BeritaVM> data)
        {
            Data = data;
        }

    }
    public class BeritaResponseGetByCreatedBy
    {
        public List<BeritaVM> Data { get; set; }

        public BeritaResponseGetByCreatedBy(List<BeritaVM> data)
        {
            Data = data;
        }

    }
    public class BeritaResponseUpdate
    {
        public string Id { get; set; }

        public BeritaResponseUpdate(string id)
        {
            Id = id;
        }

    }
    public class BeritaResponseDelete
    {
        public string Id { get; set; }

        public BeritaResponseDelete(string id)
        {
            Id = id;
        }

    }

}

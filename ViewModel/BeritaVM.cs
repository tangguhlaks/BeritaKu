namespace BeritaKuAPI.ViewModel
{
    public class BeritaVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
    public class BeritaCreateUpdateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

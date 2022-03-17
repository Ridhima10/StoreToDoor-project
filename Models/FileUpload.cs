namespace StoreToDoor.Models
{
    public class FileUpload
    {
        public string? Title { get; set; }

        public IFormFile? File { get; set; }

        public string? Size { get; set; }

        public string? Category { get; set; }

        public int Price { get; set; }
    }
}

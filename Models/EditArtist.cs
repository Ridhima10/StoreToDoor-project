namespace StoreToDoor.Models
{
    public class EditArtist
    {
        public string? AccountName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public string? Bio { get; set; }
    }
}

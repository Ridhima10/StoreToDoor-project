using Microsoft.AspNetCore.Identity;

namespace StoreToDoor.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public int? PinCode { get; set; }

        public string? Bio { get; set; }

        public string? AccountName { get; set; }

        public string? ProfileImage { get; set; } = "";
    }
}

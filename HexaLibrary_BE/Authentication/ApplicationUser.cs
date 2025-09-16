using Microsoft.AspNetCore.Identity;

namespace HexaLibrary_BE.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

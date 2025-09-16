using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Authentication
{
    public class RegisterModel
    {
        [Required]
        public string ?UserName { get; set; }
        [Required, MinLength(6)]
        public string ?Password { get; set; }
        [Required, EmailAddress]
        public string ?Email { get; set; }
        [Required]
        public string ?Role { get; set; }
        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}

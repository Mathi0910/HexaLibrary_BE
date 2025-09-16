using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Authentication
{
    public class LoginModel
    {
        [Required]
        public string ?UserName { get; set; }
        [Required]
        public string ?Password { get; set; }
    }
}

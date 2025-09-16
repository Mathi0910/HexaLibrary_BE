using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexaLibrary_BE.Models
{
    public class PasswordReset
    {
        [Key]
        public int ResetId { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, StringLength(255)]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public bool Used { get; set; } = false;

       

    }
}

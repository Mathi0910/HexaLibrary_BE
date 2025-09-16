using HexaLibrary_BE.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class PasswordReset
    {
        [Key]
        public int ResetId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public string ResetToken { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }
    }
}

using HexaLibrary_BE.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

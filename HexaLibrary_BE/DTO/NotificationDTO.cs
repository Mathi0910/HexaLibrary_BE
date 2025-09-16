
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.DTOs
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }

        [Required, StringLength(255)]
        public string Message { get; set; } = string.Empty;

        [Required]
        public DateTime SentAt { get; set; }

        [StringLength(50)]
        public string? Type { get; set; }
    }
}
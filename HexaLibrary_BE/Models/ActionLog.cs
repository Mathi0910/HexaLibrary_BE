using HexaLibrary_BE.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class ActionLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public string Action { get; set; } = string.Empty;

        [Required]
        public DateTime ActionDate { get; set; }
    }
}

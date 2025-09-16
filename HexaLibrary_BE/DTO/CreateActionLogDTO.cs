using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.DTOs
{
    public class CreateActionLogDTO
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Action { get; set; } = string.Empty;
    }
}
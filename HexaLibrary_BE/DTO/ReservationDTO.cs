using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }

        [Required]
        public string BookTitle { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
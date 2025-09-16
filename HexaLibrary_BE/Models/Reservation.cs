using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexaLibrary_BE.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;

        
        public Book? Book { get; set; }
        public string? User { get; internal set; }
    }
}

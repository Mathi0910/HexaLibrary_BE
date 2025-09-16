using HexaLibrary_BE.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required]
        public DateTime ReservationDate { get; set; }

        public bool IsFulfilled { get; set; }
    }
}

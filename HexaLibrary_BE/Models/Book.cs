using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        public string Publisher { get; set; } = string.Empty;
        public int Year { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public int TotalCopies { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        // Navigation
        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}

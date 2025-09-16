using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexaLibrary_BE.Models
{
    public class BorrowedBook
    {
        [Key]
        public int BorrowId { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Precision(10, 2)]
        public decimal? FineAmount { get; set; } = 0;

        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;

        
        public Book? Book { get; set; }

    }
}

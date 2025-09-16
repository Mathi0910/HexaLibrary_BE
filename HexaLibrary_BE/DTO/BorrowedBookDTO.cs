
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.DTOs
{
    public class BorrowedBookDTO
    {
        public int BorrowId { get; set; }

        [Required]
        public string BookTitle { get; set; } = string.Empty;

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
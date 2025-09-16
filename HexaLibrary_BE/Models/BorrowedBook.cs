using HexaLibrary_BE.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class BorrowedBook
    {
        [Key]
        public int BorrowId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; }
    }
}

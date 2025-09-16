using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexaLibrary_BE.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string ISBN { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Publisher { get; set; }

        public DateTime? PublicationDate { get; set; }

        [StringLength(50)]
        public string? Edition { get; set; }

        [StringLength(50)]
        public string? Language { get; set; }

         public int? Pages { get; set; }

        [Precision(10, 2)]
        public decimal? Cost { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        [Required, ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

    }
}

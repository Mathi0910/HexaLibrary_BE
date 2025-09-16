using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.DTOs
{
    public class BookDTO
    {
        public int BookId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        public int AvailableCopies { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

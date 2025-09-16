using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

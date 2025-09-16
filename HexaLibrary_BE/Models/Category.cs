using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

    }
}

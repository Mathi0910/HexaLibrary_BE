using System.ComponentModel.DataAnnotations;

namespace HexaLibrary_BE.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;
    }
}
namespace HexaLibrary_BE.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BookCount { get; set; }
    }
}
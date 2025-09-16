namespace HexaLibrary_BE.DTOs
{
    public class BorrowedBookDTO
    {
        public int BorrowId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}

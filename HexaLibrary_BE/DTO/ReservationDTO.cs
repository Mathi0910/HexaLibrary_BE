namespace HexaLibrary_BE.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public bool IsFulfilled { get; set; }
    }
}
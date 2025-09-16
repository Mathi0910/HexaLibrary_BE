namespace HexaLibrary_BE.DTOs
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

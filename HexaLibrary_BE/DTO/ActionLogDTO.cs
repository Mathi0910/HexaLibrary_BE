namespace HexaLibrary_BE.DTOs
{
    public class ActionLogDTO
    {
        public int LogId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

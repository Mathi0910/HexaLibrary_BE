namespace HexaLibrary_BE.DTOs
{
    public class PasswordResetDTO
    {
        public int ResetId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ResetToken { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
using HexaLibrary_BE.Models;
using Microsoft.AspNetCore.Identity;

namespace HexaLibrary_BE.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
        public ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}


using HexaLibrary_BE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 

        }

        
        public DbSet<Book> Book { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BorrowedBook> BorrowedBook { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<ActionLog> ActionLog { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<PasswordReset> PasswordReset { get; set; }
    }
}

using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId)
        {
            try
            {
                return await _context.Set<Notification>()
                                     .AsNoTracking()
                                     .Where(n => n.UserId == userId)
                                     .OrderByDescending(n => n.SentAt)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching notifications for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Set<Notification>()
                                     .AsNoTracking()
                                     .Where(n => n.UserId == userId && n.Type == "Unread")
                                     .OrderByDescending(n => n.SentAt)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching unread notifications for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(type))
                    return new List<Notification>();

                return await _context.Set<Notification>()
                                     .AsNoTracking()
                                     .Where(n => n.Type == type)
                                     .OrderByDescending(n => n.SentAt)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching notifications of type '{type}'", ex);
            }
        }
    }
}

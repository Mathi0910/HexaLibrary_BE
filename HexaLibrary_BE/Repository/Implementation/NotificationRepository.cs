using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            try
            {
                return await _context.Notifications.Include(x => x.User).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching notifications", ex);
            }
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Notifications.Include(x => x.User).FirstOrDefaultAsync(x => x.NotificationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching notification with ID {id}", ex);
            }
        }

        public async Task AddAsync(Notification notification)
        {
            try
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding notification", ex);
            }
        }

        public async Task UpdateAsync(Notification notification)
        {
            try
            {
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating notification", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification != null)
                {
                    _context.Notifications.Remove(notification);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting notification", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching notifications for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Notifications.Where(x => x.UserId == userId && !x.IsRead).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching unread notifications for user {userId}", ex);
            }
        }
    }
}

using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class ActionLogRepository : GenericRepository<ActionLog>, IActionLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ActionLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActionLog>> GetLogsByUserAsync(int userId)
        {
            try
            {
                return await _context.Set<ActionLog>()
                                     .AsNoTracking()
                                     .Where(log => log.UserId == userId)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching logs for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<ActionLog>> GetLogsByDateAsync(DateTime date)
        {
            try
            {
                return await _context.Set<ActionLog>()
                                     .AsNoTracking()
                                     .Where(log => log.ActionTime.Date == date.Date)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching logs for date {date:yyyy-MM-dd}", ex);
            }
        }

        public async Task<IEnumerable<ActionLog>> GetRecentLogsAsync(int count)
        {
            try
            {
                return await _context.Set<ActionLog>()
                                     .AsNoTracking()
                                     .OrderByDescending(log => log.ActionTime)
                                     .Take(count)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching recent {count} logs", ex);
            }
        }
    }
}

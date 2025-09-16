using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class ActionLogRepository : IActionLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ActionLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActionLog>> GetAllAsync()
        {
            try
            {
                return await _context.ActionLogs.Include(x => x.User).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching action logs", ex);
            }
        }

        public async Task<ActionLog?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.ActionLogs.Include(x => x.User).FirstOrDefaultAsync(x => x.LogId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching action log with ID {id}", ex);
            }
        }

        public async Task AddAsync(ActionLog log)
        {
            try
            {
                await _context.ActionLogs.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding action log", ex);
            }
        }

        public async Task UpdateAsync(ActionLog log)
        {
            try
            {
                _context.ActionLogs.Update(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating action log", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var log = await _context.ActionLogs.FindAsync(id);
                if (log != null)
                {
                    _context.ActionLogs.Remove(log);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting action log", ex);
            }
        }

        public async Task<IEnumerable<ActionLog>> GetLogsByUserIdAsync(string userId)
        {
            try
            {
                return await _context.ActionLogs.Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching logs for user {userId}", ex);
            }
        }
    }
}


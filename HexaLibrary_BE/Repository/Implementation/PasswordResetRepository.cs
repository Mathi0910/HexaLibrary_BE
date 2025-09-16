using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PasswordReset>> GetAllAsync()
        {
            try
            {
                return await _context.PasswordResets.Include(x => x.User).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching password reset requests", ex);
            }
        }

        public async Task<PasswordReset?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.PasswordResets.Include(x => x.User).FirstOrDefaultAsync(x => x.ResetId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching password reset request with ID {id}", ex);
            }
        }

        public async Task AddAsync(PasswordReset resetRequest)
        {
            try
            {
                await _context.PasswordResets.AddAsync(resetRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding password reset request", ex);
            }
        }

        public async Task UpdateAsync(PasswordReset resetRequest)
        {
            try
            {
                _context.PasswordResets.Update(resetRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating password reset request", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var reset = await _context.PasswordResets.FindAsync(id);
                if (reset != null)
                {
                    _context.PasswordResets.Remove(reset);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting password reset request", ex);
            }
        }

        public async Task<PasswordReset?> GetValidTokenAsync(string userId, string token)
        {
            try
            {
                return await _context.PasswordResets.FirstOrDefaultAsync(x => x.UserId == userId && x.ResetToken == token && !x.IsUsed && x.ExpiryDate > DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating password reset token", ex);
            }
        }
    }
}

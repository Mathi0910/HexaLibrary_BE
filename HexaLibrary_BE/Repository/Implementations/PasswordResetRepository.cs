using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class PasswordResetRepository : GenericRepository<PasswordReset>, IPasswordResetRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PasswordReset?> GetActiveResetTokenAsync(int userId)
        {
            try
            {
                return await _context.Set<PasswordReset>()
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(r => r.UserId == userId
                                                            && r.Used == false
                                                            && r.ExpiresAt > DateTime.Now);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching active reset token for user {userId}", ex);
            }
        }

        public async Task InvalidateOldTokensAsync(int userId)
        {
            try
            {
                var tokens = await _context.Set<PasswordReset>()
                                           .Where(r => r.UserId == userId && r.Used == false)
                                           .ToListAsync();

                foreach (var token in tokens)
                {
                    token.Used = true;
                }

                _context.UpdateRange(tokens);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error invalidating old reset tokens for user {userId}", ex);
            }
        }
    }
}

using HexaLibrary_BE.Models;
namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IPasswordResetRepository : IGenericRepository<PasswordReset>
    {
        Task<PasswordReset?> GetActiveResetTokenAsync(int userId);
        Task InvalidateOldTokensAsync(int userId);
    }
}

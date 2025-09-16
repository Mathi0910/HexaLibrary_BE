using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task<IEnumerable<PasswordReset>> GetAllAsync();
        Task<PasswordReset?> GetByIdAsync(int id);
        Task AddAsync(PasswordReset resetRequest);
        Task UpdateAsync(PasswordReset resetRequest);
        Task DeleteAsync(int id);

        // Extra
        Task<PasswordReset?> GetValidTokenAsync(string userId, string token);
    }
}

using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IActionLogRepository
    {
        Task<IEnumerable<ActionLog>> GetAllAsync();
        Task<ActionLog?> GetByIdAsync(int id);
        Task AddAsync(ActionLog log);
        Task UpdateAsync(ActionLog log);
        Task DeleteAsync(int id);

        // Extra
        Task<IEnumerable<ActionLog>> GetLogsByUserIdAsync(string userId);
    }
}

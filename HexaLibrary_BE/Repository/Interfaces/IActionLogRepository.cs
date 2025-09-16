using HexaLibrary_BE.Models;
namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IActionLogRepository : IGenericRepository<ActionLog>
    {
        Task<IEnumerable<ActionLog>> GetLogsByUserAsync(int userId);
        Task<IEnumerable<ActionLog>> GetLogsByDateAsync(DateTime date);
        Task<IEnumerable<ActionLog>> GetRecentLogsAsync(int count);
    }
}

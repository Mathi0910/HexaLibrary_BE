using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int id);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);

        // Extra
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(string userId);
    }
}

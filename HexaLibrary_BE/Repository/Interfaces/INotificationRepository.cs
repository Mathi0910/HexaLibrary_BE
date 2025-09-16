using HexaLibrary_BE.Models;
namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(string type);
    }
}

using HexaLibrary_BE.Models;
namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetReservationsByUserAsync(int userId);
        Task<IEnumerable<Reservation>> GetActiveReservationsAsync();
        Task<Reservation?> GetReservationByBookAsync(int bookId);
    }
}

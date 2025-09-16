using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(int id);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(int id);

        
        Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Reservation>> GetActiveReservationsAsync();
    }
}

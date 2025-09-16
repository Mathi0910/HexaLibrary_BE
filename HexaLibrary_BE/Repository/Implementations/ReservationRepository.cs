using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
        {
            try
            {
                return await _context.Set<Reservation>()
                                     .AsNoTracking()
                                     .Where(r => r.Status == "Active")
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching active reservations", ex);
            }
        }

        public async Task<Reservation?> GetReservationByBookAsync(int bookId)
        {
            try
            {
                return await _context.Set<Reservation>()
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(r => r.BookId == bookId && r.Status == "Active");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching reservation for book {bookId}", ex);
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserAsync(int userId)
        {
            try
            {
                return await _context.Set<Reservation>()
                                     .AsNoTracking()
                                     .Where(r => r.UserId == userId)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching reservations for user {userId}", ex);
            }
        }
    }
}

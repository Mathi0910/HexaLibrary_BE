using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            try
            {
                return await _context.Reservations.Include(x => x.User).Include(x => x.Book).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching reservations", ex);
            }
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Reservations.Include(x => x.User).Include(x => x.Book).FirstOrDefaultAsync(x => x.ReservationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching reservation with ID {id}", ex);
            }
        }

        public async Task AddAsync(Reservation reservation)
        {
            try
            {
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding reservation", ex);
            }
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            try
            {
                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating reservation", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation != null)
                {
                    _context.Reservations.Remove(reservation);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting reservation", ex);
            }
        }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Reservations.Include(x => x.Book).Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching reservations for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
        {
            try
            {
                return await _context.Reservations.Where(x => !x.IsFulfilled).Include(x => x.Book).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching active reservations", ex);
            }
        }
    }
}


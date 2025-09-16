using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class BorrowedBookRepository : GenericRepository<BorrowedBook>, IBorrowedBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowedBookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowedBook>> GetOverdueBooksAsync()
        {
            try
            {
                return await _context.Set<BorrowedBook>()
                                     .AsNoTracking()
                                     .Include(b => b.Book)
                                     .Where(b => b.ReturnDate == null && b.DueDate < DateTime.Now)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching overdue borrowed books", ex);
            }
        }

        public async Task<IEnumerable<BorrowedBook>> GetBorrowedBookByUserAsync(int userId)
        {
            try
            {
                return await _context.Set<BorrowedBook>()
                                     .AsNoTracking()
                                     .Include(b => b.Book)
                                     .Where(b => b.UserId == userId && b.ReturnDate == null)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching active borrowed books for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<BorrowedBook>> GetActiveBorrowingsAsync()
        {
            try
            {
                return await _context.Set<BorrowedBook>()
                                     .AsNoTracking()
                                     .Include(b => b.Book)
                                     .Where(b => b.ReturnDate == null)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching active borrowings", ex);
            }
        }

        public async Task<IEnumerable<BorrowedBook>> GetBorrowingHistoryAsync(int userId)
        {
            try
            {
                return await _context.Set<BorrowedBook>()
                                     .AsNoTracking()
                                     .Include(b => b.Book)
                                     .Where(b => b.UserId == userId)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching borrowing history for user {userId}", ex);
            }
        }
    }
}

using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowedBookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowedBook>> GetAllAsync()
        {
            try
            {
                return await _context.BorrowedBooks.Include(x => x.User).Include(x => x.Book).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching borrowed books", ex);
            }
        }

        public async Task<BorrowedBook?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.BorrowedBooks.Include(x => x.User).Include(x => x.Book).FirstOrDefaultAsync(x => x.BorrowId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching borrowed book with ID {id}", ex);
            }
        }

        public async Task AddAsync(BorrowedBook borrowedBook)
        {
            try
            {
                await _context.BorrowedBooks.AddAsync(borrowedBook);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding borrowed book", ex);
            }
        }

        public async Task UpdateAsync(BorrowedBook borrowedBook)
        {
            try
            {
                _context.BorrowedBooks.Update(borrowedBook);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating borrowed book", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var borrow = await _context.BorrowedBooks.FindAsync(id);
                if (borrow != null)
                {
                    _context.BorrowedBooks.Remove(borrow);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting borrowed book", ex);
            }
        }

        public async Task<IEnumerable<BorrowedBook>> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _context.BorrowedBooks.Include(x => x.Book).Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching borrowed books for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<BorrowedBook>> GetActiveBorrowsAsync()
        {
            try
            {
                return await _context.BorrowedBooks.Where(x => !x.IsReturned).Include(x => x.Book).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching active borrows", ex);
            }
        }
    }
}

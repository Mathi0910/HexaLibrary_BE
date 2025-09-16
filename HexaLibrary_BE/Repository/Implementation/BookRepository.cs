using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            try
            {
                return await _context.Books.Include(x => x.Category).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching books", ex);
            }
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Books.Include(x => x.Category).FirstOrDefaultAsync(x => x.BookId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching book with ID {id}", ex);
            }
        }

        public async Task AddAsync(Book book)
        {
            try
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding book", ex);
            }
        }

        public async Task UpdateAsync(Book book)
        {
            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating book", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting book", ex);
            }
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Books
                    .Include(x => x.Category)   // ✅ load Category too
                    .Where(x => x.CategoryId == categoryId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching books for category {categoryId}", ex);
            }
        }
    }
}

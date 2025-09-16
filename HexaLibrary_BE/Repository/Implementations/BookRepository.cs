using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Book?> GetBookByIsbnAsync(string isbn)
        {
            try
            {
                if (string.IsNullOrEmpty(isbn))
                    return null;

                return await _context.Set<Book>()
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(b => b.ISBN == isbn);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching book with ISBN {isbn}", ex);
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Set<Book>()
                                     .AsNoTracking()
                                     .Where(b => b.CategoryId == categoryId)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching books for category {categoryId}", ex);
            }
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            try
            {
                return await _context.Set<Book>()
                                     .AsNoTracking()
                                     .Where(b => b.AvailableCopies == 0)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching available books", ex);
            }
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                    return new List<Book>();

                keyword = keyword.ToLower();

                return await _context.Set<Book>()
                                     .AsNoTracking()
                                     .Where(b => b.Title.ToLower().Contains(keyword) ||
                                                 b.Author.ToLower().Contains(keyword)) // ✅ fixed Author check
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching books with keyword '{keyword}'", ex);
            }
        }
    }
}

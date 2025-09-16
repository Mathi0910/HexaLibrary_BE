using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                return await _context.Categories.Include(x => x.Books).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching categories", ex);
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Categories.Include(x => x.Books).FirstOrDefaultAsync(x => x.CategoryId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching category with ID {id}", ex);
            }
        }

        public async Task AddAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category", ex);
            }
        }

        public async Task UpdateAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category", ex);
            }
        }
    }
}

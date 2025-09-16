using HexaLibrary_BE.Authentication;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary_BE.Repository.Implementations
{
    public class CategoryBookRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryBookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    return null;

                return await _context.Set<Category>()
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching category with name '{categoryName}'", ex);
            }
        }
    }
}

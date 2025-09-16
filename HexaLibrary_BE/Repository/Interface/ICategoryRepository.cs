using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}

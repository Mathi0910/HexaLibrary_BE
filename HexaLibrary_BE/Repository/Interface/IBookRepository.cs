using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);

        // Extra
        Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
    }
}

using HexaLibrary_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IBorrowedBookRepository
    {
        Task<IEnumerable<BorrowedBook>> GetAllAsync();
        Task<BorrowedBook?> GetByIdAsync(int id);
        Task AddAsync(BorrowedBook borrowedBook);
        Task UpdateAsync(BorrowedBook borrowedBook);
        Task DeleteAsync(int id);

        // Extra
        Task<IEnumerable<BorrowedBook>> GetByUserIdAsync(string userId);
        Task<IEnumerable<BorrowedBook>> GetActiveBorrowsAsync();
    }
}
using HexaLibrary_BE.Models;
namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface IBorrowedBookRepository : IGenericRepository<BorrowedBook>
    {
        Task<IEnumerable<BorrowedBook>> GetOverdueBooksAsync();
        Task<IEnumerable<BorrowedBook>> GetBorrowedBookByUserAsync(int userId);
        Task<IEnumerable<BorrowedBook>> GetActiveBorrowingsAsync();
        Task<IEnumerable<BorrowedBook>> GetBorrowingHistoryAsync(int userId);
    }
}

using HexaLibrary_BE.Models;
namespace HexaLibrary_BE.Repository.Interfaces
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByNameAsync(string categoryName);
        
    }
}

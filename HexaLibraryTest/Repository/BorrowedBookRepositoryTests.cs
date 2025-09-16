using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HexaLibrary.Tests
{
    public class BorrowedBookRepositoryTests : TestBase
    {
        [Test]
        public async Task AddBorrowedBook_ShouldAddRecord()
        {
            var repo = new BorrowedBookRepository(_context);
            var borrowed = new BorrowedBook
            {
                UserId = "test-user",
                BookId = 1,
               
            };

            await repo.AddAsync(borrowed);
            await _context.SaveChangesAsync(); // ✅ ensure BorrowId is generated

            Assert.That(_context.BorrowedBooks.Count(), Is.EqualTo(1));
            
        }

        
    }
}
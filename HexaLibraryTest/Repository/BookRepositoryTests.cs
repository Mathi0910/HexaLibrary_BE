using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HexaLibrary.Tests
{
    public class BookRepositoryTests : TestBase
    {
        [Test]
        public async Task AddBook_ShouldAddBook()
        {
            var repo = new BookRepository(_context);
            var book = new Book { Title = "C# Fundamentals", Author = "Mathiii", CategoryId = 1 };

            await repo.AddAsync(book);
            await _context.SaveChangesAsync(); // ✅ ensure BookId is generated

            Assert.That(_context.Books.Count(), Is.EqualTo(1));
            Assert.That(_context.Books.First().Title, Is.EqualTo("C# Fundamentals"));
        }

       
    }
}
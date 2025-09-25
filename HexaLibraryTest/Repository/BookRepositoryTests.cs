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
            // Arrange
            var category = new Category { Name = "Programming" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var repo = new BookRepository(_context);
            var book = new Book
            {
                Title = "C# Basics",
                Author = "Mathiii",
                Publisher = "TechPub",
                Year = 2025,
                CategoryId = category.CategoryId,
                TotalCopies = 10,
                AvailableCopies = 10
            };

            // Act
            await repo.AddAsync(book);

            // Assert
            Assert.That(_context.Books.Count(), Is.EqualTo(1));
            Assert.That(_context.Books.First().Title, Is.EqualTo("C# Basics"));
        }

        [Test]
        public async Task GetBookById_ShouldReturnBook()
        {
            var category = new Category { Name = "Databases" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var repo = new BookRepository(_context);
            var book = new Book
            {
                Title = "C# Advanced",
                Author = "Mathiii",
                Publisher = "TechPub",
                Year = 2025,
                CategoryId = category.CategoryId,
                TotalCopies = 5,
                AvailableCopies = 5
            };

            await repo.AddAsync(book);

            var result = await repo.GetByIdAsync(book.BookId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("C# Advanced"));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllBooks()
        {
            var category = new Category { Name = "Cloud" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var repo = new BookRepository(_context);

            await repo.AddAsync(new Book
            {
                Title = "Book A",
                Author = "Author A",
                Publisher = "Pub A",
                Year = 2020,
                CategoryId = category.CategoryId,
                TotalCopies = 3,
                AvailableCopies = 3
            });

            await repo.AddAsync(new Book
            {
                Title = "Book B",
                Author = "Author B",
                Publisher = "Pub B",
                Year = 2021,
                CategoryId = category.CategoryId,
                TotalCopies = 2,
                AvailableCopies = 2
            });

            var result = await repo.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateBook_ShouldModifyBook()
        {
            var category = new Category { Name = "Networks" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var repo = new BookRepository(_context);
            var book = new Book
            {
                Title = "Old Title",
                Author = "Mathiii",
                Publisher = "TechPub",
                Year = 2023,
                CategoryId = category.CategoryId,
                TotalCopies = 4,
                AvailableCopies = 4
            };

            await repo.AddAsync(book);

            book.Title = "New Title";
            await repo.UpdateAsync(book);

            var updated = await repo.GetByIdAsync(book.BookId);
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.Title, Is.EqualTo("New Title"));
        }

        [Test]
        public async Task DeleteBook_ShouldRemoveBook()
        {
            var category = new Category { Name = "Security" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var repo = new BookRepository(_context);
            var book = new Book
            {
                Title = "Delete Me",
                Author = "Mathiii",
                Publisher = "TechPub",
                Year = 2022,
                CategoryId = category.CategoryId,
                TotalCopies = 1,
                AvailableCopies = 1
            };

            await repo.AddAsync(book);
            var bookId = book.BookId;

            await repo.DeleteAsync(bookId);

            var deleted = await repo.GetByIdAsync(bookId);
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task GetByCategory_ShouldReturnBooksForCategory()
        {
            var category1 = new Category { Name = "AI" };
            var category2 = new Category { Name = "ML" };
            _context.Categories.AddRange(category1, category2);
            await _context.SaveChangesAsync();

            var repo = new BookRepository(_context);

            await repo.AddAsync(new Book
            {
                Title = "Category Book 1",
                Author = "Author 1",
                Publisher = "Pub 1",
                Year = 2021,
                CategoryId = category1.CategoryId,
                TotalCopies = 5,
                AvailableCopies = 5
            });

            await repo.AddAsync(new Book
            {
                Title = "Category Book 2",
                Author = "Author 2",
                Publisher = "Pub 2",
                Year = 2022,
                CategoryId = category1.CategoryId,
                TotalCopies = 3,
                AvailableCopies = 3
            });

            await repo.AddAsync(new Book
            {
                Title = "Other Category Book",
                Author = "Author 3",
                Publisher = "Pub 3",
                Year = 2023,
                CategoryId = category2.CategoryId,
                TotalCopies = 2,
                AvailableCopies = 2
            });

            var result = await repo.GetByCategoryAsync(category1.CategoryId);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(b => b.CategoryId == category1.CategoryId), Is.True);
        }
    }
}
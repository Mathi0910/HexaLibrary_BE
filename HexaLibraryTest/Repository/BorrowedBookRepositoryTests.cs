using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HexaLibrary.Tests
{
    public class BorrowedBookRepositoryTests : TestBase
    {
        [Test]
        public async Task AddBorrowedBook_ShouldAddRecord()
        {
            // Arrange: create category and book so relationships work
            var category = new Category { Name = "Programming" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Title = "C# Fundamentals",
                Author = "Mathiii",
                Publisher = "TechPub",
                Year = 2025,
                CategoryId = category.CategoryId,
                TotalCopies = 5,
                AvailableCopies = 5
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var repo = new BorrowedBookRepository(_context);
            var borrowed = new BorrowedBook
            {
                UserId = "test-user",
                BookId = book.BookId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(7),
                IsReturned = false
            };

            // Act
            await repo.AddAsync(borrowed);

            // Assert
            Assert.That(_context.BorrowedBooks.Count(), Is.EqualTo(1));

            // Fetch the saved record including Book navigation to check title
            var saved = await _context.BorrowedBooks.Include(b => b.Book).FirstOrDefaultAsync();
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved!.Book, Is.Not.Null);
            Assert.That(saved.Book.Title, Is.EqualTo("C# Fundamentals"));
        }

        
             

        

        [Test]
        public async Task GetByUserId_ShouldReturnBorrowedBooksForUser()
        {
            // Arrange
            var category = new Category { Name = "Networking" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Title = "Networking Basics",
                Author = "Author Net",
                Publisher = "Pub Net",
                Year = 2021,
                CategoryId = category.CategoryId,
                TotalCopies = 2,
                AvailableCopies = 2
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var repo = new BorrowedBookRepository(_context);

            await repo.AddAsync(new BorrowedBook
            {
                UserId = "special-user",
                BookId = book.BookId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(7),
                IsReturned = false
            });

            // Act
            var result = await repo.GetByUserIdAsync("special-user");

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().UserId, Is.EqualTo("special-user"));
            Assert.That(result.First().Book, Is.Not.Null);
            Assert.That(result.First().Book.Title, Is.EqualTo("Networking Basics"));
        }

        [Test]
        public async Task GetActiveBorrows_ShouldReturnOnlyNotReturned()
        {
            // Arrange
            var category = new Category { Name = "ML" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Title = "Machine Learning",
                Author = "AI Author",
                Publisher = "Pub ML",
                Year = 2020,
                CategoryId = category.CategoryId,
                TotalCopies = 4,
                AvailableCopies = 4
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var repo = new BorrowedBookRepository(_context);

            await repo.AddAsync(new BorrowedBook
            {
                UserId = "ml-user",
                BookId = book.BookId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(7),
                IsReturned = false
            });

            await repo.AddAsync(new BorrowedBook
            {
                UserId = "ml-user",
                BookId = book.BookId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(7),
                IsReturned = true
            });

            // Act
            var result = await repo.GetActiveBorrowsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().IsReturned, Is.False);
            Assert.That(result.First().Book, Is.Not.Null);
        }

       
             
        [Test]
        public async Task DeleteBorrowedBook_ShouldRemoveRecord()
        {
            // Arrange
            var category = new Category { Name = "Security" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Title = "Cyber Security",
                Author = "Sec Author",
                Publisher = "Pub Sec",
                Year = 2023,
                CategoryId = category.CategoryId,
                TotalCopies = 1,
                AvailableCopies = 1
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var repo = new BorrowedBookRepository(_context);
            var borrowed = new BorrowedBook
            {
                UserId = "delete-user",
                BookId = book.BookId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(7),
                IsReturned = false
            };

            await repo.AddAsync(borrowed);
            var borrowId = borrowed.BorrowId;

            // Act
            await repo.DeleteAsync(borrowId);

            // Assert
            var deleted = await repo.GetByIdAsync(borrowId);
            Assert.That(deleted, Is.Null);
        }
    }
}
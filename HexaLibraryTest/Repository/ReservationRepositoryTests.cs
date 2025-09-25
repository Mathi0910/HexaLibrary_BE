using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary.Tests
{
    public class ReservationRepositoryTests : TestBase
    {
        private async Task<Book> SeedBook(string title)
        {
            var category = new Category { Name = "TestCategory" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Title = title,
                Author = "Test Author",
                Publisher = "Test Pub",
                Year = 2025,
                CategoryId = category.CategoryId,
                TotalCopies = 5,
                AvailableCopies = 5
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        [Test]
        public async Task AddReservation_ShouldAddReservation()
        {
            var book = await SeedBook("C# Basics");
            var repo = new ReservationRepository(_context);

            var reservation = new Reservation
            {
                UserId = "test-user",
                BookId = book.BookId,
                Book = book, // ✅ Attach book
                ReservationDate = DateTime.UtcNow,
                IsFulfilled = false
            };

            await repo.AddAsync(reservation);

            var saved = await _context.Reservations.Include(r => r.Book).FirstOrDefaultAsync();
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved!.Book.Title, Is.EqualTo("C# Basics"));
        }

        

      

        [Test]
        public async Task GetByUserId_ShouldReturnReservationsForUser()
        {
            var book = await SeedBook("Networking Basics");
            var repo = new ReservationRepository(_context);

            await repo.AddAsync(new Reservation
            {
                UserId = "special-user",
                BookId = book.BookId,
                Book = book,
                ReservationDate = DateTime.UtcNow,
                IsFulfilled = false
            });

            var result = await repo.GetByUserIdAsync("special-user");

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().UserId, Is.EqualTo("special-user"));
            Assert.That(result.First().Book.Title, Is.EqualTo("Networking Basics"));
        }

        [Test]
        public async Task GetActiveReservations_ShouldReturnOnlyNotFulfilled()
        {
            var book = await SeedBook("Machine Learning");
            var repo = new ReservationRepository(_context);

            await repo.AddAsync(new Reservation
            {
                UserId = "ml-user",
                BookId = book.BookId,
                Book = book,
                ReservationDate = DateTime.UtcNow,
                IsFulfilled = false
            });

            await repo.AddAsync(new Reservation
            {
                UserId = "ml-user",
                BookId = book.BookId,
                Book = book,
                ReservationDate = DateTime.UtcNow,
                IsFulfilled = true
            });

            var result = await repo.GetActiveReservationsAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().IsFulfilled, Is.False);
        }

       

        [Test]
        public async Task DeleteReservation_ShouldRemoveReservation()
        {
            var book = await SeedBook("Delete Me");
            var repo = new ReservationRepository(_context);

            var reservation = new Reservation
            {
                UserId = "delete-user",
                BookId = book.BookId,
                Book = book,
                ReservationDate = DateTime.UtcNow,
                IsFulfilled = false
            };

            await repo.AddAsync(reservation);
            var reservationId = reservation.ReservationId;

            await repo.DeleteAsync(reservationId);

            var deleted = await repo.GetByIdAsync(reservationId);
            Assert.That(deleted, Is.Null);
        }
    }
}
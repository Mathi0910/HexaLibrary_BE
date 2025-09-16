using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HexaLibrary.Tests
{
    public class ReservationRepositoryTests : TestBase
    {
        [Test]
        public async Task AddReservation_ShouldAddReservation()
        {
            var repo = new ReservationRepository(_context);
            var reservation = new Reservation
            {
                UserId = "test-user",
                BookId = 1,
                ReservationDate = System.DateTime.Now,
                IsFulfilled = false
            };

            await repo.AddAsync(reservation);
            await _context.SaveChangesAsync(); // ✅ ensure ReservationId is generated

            Assert.That(_context.Reservations.Count(), Is.EqualTo(1));
            Assert.That(_context.Reservations.First().UserId, Is.EqualTo("test-user"));
        }

        
    }
}
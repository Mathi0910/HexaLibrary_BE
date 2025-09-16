using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IBookRepository _bookRepo;

        public ReservationController(IReservationRepository reservationRepo, IBookRepository bookRepo)
        {
            _reservationRepo = reservationRepo;
            _bookRepo = bookRepo;
        }

        // POST: api/reservation/reserve
        [HttpPost("reserve")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> ReserveBook(int userId, int bookId)
        {
            try
            {
                var book = await _bookRepo.GetByIdAsync(bookId);
                if (book == null) return NotFound(new { Message = "Book not found" });

                // Only allow reservation if no copies available
                if (book.AvailableCopies > 0)
                    return BadRequest(new { Message = "Book is available, no need to reserve" });

                var existing = await _reservationRepo.GetReservationByBookAsync(bookId);
                if (existing != null && existing.UserId == userId && existing.Status == "Active")
                    return BadRequest(new { Message = "You already reserved this book" }); 
                var reservation = new Reservation
                {
                    UserId = userId,
                    BookId = bookId,
                    ReservationDate = DateTime.Now,
                    Status = "Active"
                };

                await _reservationRepo.AddAsync(reservation);
                return Ok(new { Message = "Book reserved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error reserving book", Details = ex.Message });
            }
        }

        // POST: api/reservation/cancel/{reservationId}
        [HttpPost("cancel/{reservationId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepo.GetByIdAsync(reservationId);
                if (reservation == null) return NotFound(new { Message = "Reservation not found" });

                reservation.Status = "Cancelled";
                await _reservationRepo.UpdateAsync(reservation);

                return Ok(new { Message = "Reservation cancelled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error cancelling reservation", Details = ex.Message });
            }
        }

        // GET: api/reservation/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetByUser(int userId)
        {
            try
            {
                var reservations = await _reservationRepo.GetReservationsByUserAsync(userId);
                if (reservations == null || !reservations.Any())
                    return NotFound(new { Message = $"No reservations found for user {userId}" });

                var dtos = reservations.Select(r => new ReservationDTO
                {
                    ReservationId = r.ReservationId,
                    BookTitle = r.Book?.Title ?? "Unknown",
                    UserName = "N/A",
                    ReservationDate = r.ReservationDate,
                    Status = r.Status
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching reservations by user", Details = ex.Message });
            }
        }

        // GET: api/reservation/active
        [HttpGet("active")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetActive()
        {
            try
            {
                var reservations = await _reservationRepo.GetActiveReservationsAsync();
                if (reservations == null || !reservations.Any())
                    return NotFound(new { Message = "No active reservations found" });

                var dtos = reservations.Select(r => new ReservationDTO
                {
                    ReservationId = r.ReservationId,
                    BookTitle = r.Book?.Title ?? "Unknown",
                    UserName = "N/A",
                    ReservationDate = r.ReservationDate,
                    Status = r.Status
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching active reservations", Details = ex.Message });
            }
        }
    }
}

using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //  Require authentication unless overridden
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepo;

        public ReservationController(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        //  Only Admin & Librarian can view all reservations
        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll()
        {
            try
            {
                var reservations = await _reservationRepo.GetAllAsync();
                var dto = reservations.Select(x => new ReservationDTO
                {
                    ReservationId = x.ReservationId,
                    UserId = x.UserId,
                    BookTitle = x.Book.Title,
                    ReservationDate = x.ReservationDate,
                    IsFulfilled = x.IsFulfilled
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin & Librarian can view by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<ReservationDTO>> GetById(int id)
        {
            try
            {
                var reservation = await _reservationRepo.GetByIdAsync(id);
                if (reservation == null) return NotFound($"Reservation with ID {id} not found");

                var dto = new ReservationDTO
                {
                    ReservationId = reservation.ReservationId,
                    UserId = reservation.UserId,
                    BookTitle = reservation.Book.Title,
                    ReservationDate = reservation.ReservationDate,
                    IsFulfilled = reservation.IsFulfilled
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin & Librarian can create reservations
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Create(ReservationDTO dto)
        {
            try
            {
                var reservation = new Reservation
                {
                    UserId = dto.UserId,
                    BookId = dto.BookId,
                    ReservationDate = dto.ReservationDate,
                    IsFulfilled = dto.IsFulfilled
                };

                await _reservationRepo.AddAsync(reservation);

                var responseDto = new ReservationDTO
                {
                    ReservationId = reservation.ReservationId,
                    UserId = reservation.UserId,
                    BookTitle = reservation.Book?.Title ?? dto.BookTitle ?? string.Empty,
                    ReservationDate = reservation.ReservationDate,
                    IsFulfilled = reservation.IsFulfilled
                };
                return CreatedAtAction(nameof(GetById), new { id = reservation.ReservationId }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding reservation: {ex.Message}");
            }
        }

        //  Only Admin & Librarian can update reservations
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Update(int id, ReservationDTO dto)
        {
            try
            {
                var existing = await _reservationRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Reservation with ID {id} not found");

                existing.ReservationDate = dto.ReservationDate;
                existing.IsFulfilled = dto.IsFulfilled;

                await _reservationRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin can delete reservations
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _reservationRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Reservation with ID {id} not found");

                await _reservationRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Normal users can only view their own reservations
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetByUserId(string userId)
        {
            try
            {
                if (!User.IsInRole("Admin") && !User.IsInRole("Librarian"))
                {
                    var currentUserId = User?.Identity?.Name;
                    if (currentUserId != userId) return Forbid();
                }

                var reservations = await _reservationRepo.GetByUserIdAsync(userId);
                var dto = reservations.Select(x => new ReservationDTO
                {
                    ReservationId = x.ReservationId,
                    UserId = x.UserId,
                    BookTitle = x.Book.Title,
                    ReservationDate = x.ReservationDate,
                    IsFulfilled = x.IsFulfilled
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Any authenticated user can view active reservations
        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetActiveReservations()
        {
            try
            {
                var reservations = await _reservationRepo.GetActiveReservationsAsync();
                var dto = reservations.Select(x => new ReservationDTO
                {
                    ReservationId = x.ReservationId,
                    UserId = x.UserId,
                    BookTitle = x.Book.Title,
                    ReservationDate = x.ReservationDate,
                    IsFulfilled = x.IsFulfilled
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //  Require authentication by default
    public class BorrowedBookController : ControllerBase
    {
        private readonly IBorrowedBookRepository _borrowedBookRepo;

        public BorrowedBookController(IBorrowedBookRepository borrowedBookRepo)
        {
            _borrowedBookRepo = borrowedBookRepo;
        }

        //  Only Admin & Librarian can see all borrowed books
        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<IEnumerable<BorrowedBookDTO>>> GetAll()
        {
            try
            {
                var borrows = await _borrowedBookRepo.GetAllAsync();
                var dto = borrows.Select(x => new BorrowedBookDTO
                {
                    BorrowId = x.BorrowId,
                    UserId = x.UserId,
                    BookTitle = x.Book.Title,
                    BorrowDate = x.BorrowDate,
                    ReturnDate = x.ReturnDate,
                    IsReturned = x.IsReturned
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Admin/Librarian can fetch by id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<BorrowedBookDTO>> GetById(int id)
        {
            try
            {
                var borrow = await _borrowedBookRepo.GetByIdAsync(id);
                if (borrow == null) return NotFound($"Borrowed book with ID {id} not found");

                var dto = new BorrowedBookDTO
                {
                    BorrowId = borrow.BorrowId,
                    UserId = borrow.UserId,
                    BookTitle = borrow.Book.Title,
                    BorrowDate = borrow.BorrowDate,
                    ReturnDate = borrow.ReturnDate,
                    IsReturned = borrow.IsReturned
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin/Librarian can create borrow records
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Create([FromBody] BorrowedBookDTO dto)
        {
            try
            {
                var borrow = new BorrowedBook
                {
                    UserId = dto.UserId,
                    BookId = dto.BookId, 
                    //BookTitle = dto.BookTitle,
                    BorrowDate = dto.BorrowDate,
                    ReturnDate = dto.ReturnDate,
                    IsReturned = dto.IsReturned
                };

                await _borrowedBookRepo.AddAsync(borrow);

                // fetch the Book title safely after save
                var bookEntity = await _borrowedBookRepo.GetByIdAsync(borrow.BorrowId);

                var responseDto = new BorrowedBookDTO
                {
                    BorrowId = borrow.BorrowId,
                    UserId = borrow.UserId,
                    BookId = borrow.BookId,
                    BookTitle = bookEntity?.Book?.Title ?? string.Empty,  //  fetch title from DB
                    BorrowDate = borrow.BorrowDate,
                    ReturnDate = borrow.ReturnDate,
                    IsReturned = borrow.IsReturned
                };

                return CreatedAtAction(nameof(GetById), new { id = borrow.BorrowId }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin/Librarian can update borrow records
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Update(int id, BorrowedBookDTO dto)
        {
            try
            {
                var existing = await _borrowedBookRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Borrowed book with ID {id} not found");

                existing.BorrowDate = dto.BorrowDate;
                existing.ReturnDate = dto.ReturnDate;
                existing.IsReturned = dto.IsReturned;

                await _borrowedBookRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin can delete borrow records
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _borrowedBookRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Borrowed book with ID {id} not found");

                await _borrowedBookRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Normal users can view only their own borrow records
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BorrowedBookDTO>>> GetByUserId(string userId)
        {
            try
            {
                if (!User.IsInRole("Admin") && !User.IsInRole("Librarian"))
                {
                    var currentUserId = User?.Identity?.Name;
                    if (currentUserId != userId) return Forbid();
                }

                var borrows = await _borrowedBookRepo.GetByUserIdAsync(userId);
                var dto = borrows.Select(x => new BorrowedBookDTO
                {
                    BorrowId = x.BorrowId,
                    UserId = x.UserId,
                    BookTitle = x.Book.Title,
                    BorrowDate = x.BorrowDate,
                    ReturnDate = x.ReturnDate,
                    IsReturned = x.IsReturned
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Any authenticated user can view active borrows
        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BorrowedBookDTO>>> GetActiveBorrows()
        {
            try
            {
                var borrows = await _borrowedBookRepo.GetActiveBorrowsAsync();
                var dto = borrows.Select(x => new BorrowedBookDTO
                {
                    BorrowId = x.BorrowId,
                    UserId = x.UserId,
                    BookTitle = x.Book.Title,
                    BorrowDate = x.BorrowDate,
                    ReturnDate = x.ReturnDate,
                    IsReturned = x.IsReturned
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

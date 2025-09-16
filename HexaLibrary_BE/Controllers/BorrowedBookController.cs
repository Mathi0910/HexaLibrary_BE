using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowedBookController : ControllerBase
    {
        private readonly IBorrowedBookRepository _borrowRepo;
        private readonly IBookRepository _bookRepo;

        public BorrowedBookController(IBorrowedBookRepository borrowRepo, IBookRepository bookRepo)
        {
            _borrowRepo = borrowRepo;
            _bookRepo = bookRepo;
        }

        // POST: api/borrowedbook/borrow
        [HttpPost("borrow")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BorrowBook(int userId, int bookId)
        {
            try
            {
                var book = await _bookRepo.GetByIdAsync(bookId);
                if (book == null || book.AvailableCopies <= 0)
                    return BadRequest(new { Message = "Book not available" });

                var borrow = new BorrowedBook
                {
                    UserId = userId,
                    BookId = bookId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14), // 2 weeks
                    Status = "Borrowed",
                    FineAmount = 0
                };

                await _borrowRepo.AddAsync(borrow);

                // decrease available copies
                book.AvailableCopies -= 1;
                await _bookRepo.UpdateAsync(book);

                return Ok(new { Message = "Book borrowed successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error borrowing book", Details = ex.Message });
            }
        }

        // POST: api/borrowedbook/return
        [HttpPost("return")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            try
            {
                var borrow = await _borrowRepo.GetByIdAsync(borrowId);
                if (borrow == null || borrow.ReturnDate != null)
                    return BadRequest(new { Message = "Invalid borrow record" });

                borrow.ReturnDate = DateTime.Now;
                borrow.Status = "Returned";

                // fine calculation
                if (borrow.ReturnDate > borrow.DueDate)
                {
                    var overdueDays = (borrow.ReturnDate.Value - borrow.DueDate).Days;
                    borrow.FineAmount = overdueDays * 5; // ₹5 per day fine
                }

                await _borrowRepo.UpdateAsync(borrow);

                // increase available copies
                var book = await _bookRepo.GetByIdAsync(borrow.BookId);
                if (book != null)
                {
                    book.AvailableCopies += 1;
                    await _bookRepo.UpdateAsync(book);
                }

                return Ok(new { Message = "Book returned successfully!", Fine = borrow.FineAmount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error returning book", Details = ex.Message });
            }
        }

        // GET: api/borrowedbook/history/{userId}
        [HttpGet("history/{userId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<BorrowedBookDTO>>> GetUserHistory(int userId)
        {
            try
            {
                var history = await _borrowRepo.GetBorrowingHistoryAsync(userId);
                if (history == null || !history.Any())
                    return NotFound(new { Message = $"No borrowing history found for user {userId}" });

                var dtos = history.Select(b => new BorrowedBookDTO
                {
                    BorrowId = b.BorrowId,
                    BookTitle = b.Book?.Title ?? "Unknown",
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching borrowing history", Details = ex.Message });
            }
        }

        // GET: api/borrowedbook/overdue
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BorrowedBookDTO>>> GetOverdueBooks()
        {
            try
            {
                var overdue = await _borrowRepo.GetOverdueBooksAsync();
                if (overdue == null || !overdue.Any())
                    return NotFound(new { Message = "No overdue books found" });

                var dtos = overdue.Select(b => new BorrowedBookDTO
                {
                    BorrowId = b.BorrowId,
                    BookTitle = b.Book?.Title ?? "Unknown",
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching overdue books", Details = ex.Message });
            }
        }
    }
}

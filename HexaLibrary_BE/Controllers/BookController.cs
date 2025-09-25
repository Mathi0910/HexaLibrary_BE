using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //  Require authentication for all actions unless overridden
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        //  Any authenticated user can see all books
        [HttpGet]
        [AllowAnonymous] //  make it public if you want open access
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll()
        {
            try
            {
                var books = await _bookRepo.GetAllAsync();
                var dto = books.Select(x => new BookDTO
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    Author = x.Author,
                    CategoryName = x.Category.Name,
                    AvailableCopies = x.AvailableCopies
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Any authenticated user can view by id
        [HttpGet("{id}")]
        [AllowAnonymous] // (optional) public access
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            try
            {
                var book = await _bookRepo.GetByIdAsync(id);
                if (book == null) return NotFound($"Book with ID {id} not found");

                var dto = new BookDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    CategoryId = book.CategoryId,
                    CategoryName = book.Category.Name,
                    AvailableCopies = book.AvailableCopies
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin & Librarian can create books
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Create(BookDTO dto)
        {
            try
            {
                var book = new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    CategoryId = dto.CategoryId,  //  use actual category id
                    AvailableCopies = dto.AvailableCopies
                };

                //  Use the saved book entity, not dto
                var responseDto = new BookDTO
                {
                    BookId = book.BookId,  // now reflects actual DB value
                    Title = book.Title,
                    Author = book.Author,
                    CategoryId = book.CategoryId,
                    CategoryName = book.Category?.Name ?? string.Empty,
                    AvailableCopies = book.AvailableCopies
                };

                await _bookRepo.AddAsync(book);
                return CreatedAtAction(nameof(GetById), new { id = book.BookId }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin & Librarian can update books
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Update(int id, BookDTO dto)
        {
            try
            {
                var existing = await _bookRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Book with ID {id} not found");

                existing.Title = dto.Title;
                existing.Author = dto.Author;
                existing.AvailableCopies = dto.AvailableCopies;

                await _bookRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Only Admin can delete books
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _bookRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Book with ID {id} not found");

                await _bookRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //  Any authenticated user can view books by category
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous] // (optional) public
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetByCategory(int categoryId)
        {
            try
            {
                var books = await _bookRepo.GetByCategoryAsync(categoryId);
                var dto = books.Select(x => new BookDTO
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    Author = x.Author,
                    CategoryName = x.Category.Name ?? string.Empty,
                    AvailableCopies = x.AvailableCopies
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

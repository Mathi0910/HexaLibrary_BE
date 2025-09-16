using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: api/book
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();
                var dtos = books.Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    AvailableCopies = b.AvailableCopies,
                    CategoryId = b.CategoryId,
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching books", Details = ex.Message });
            }
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null) return NotFound(new { Message = "Book not found" });

                var dto = new BookDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    AvailableCopies = book.AvailableCopies,
                    CategoryId = book.CategoryId,
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching book by ID", Details = ex.Message });
            }
        }

        // GET: api/book/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetByCategory(int categoryId)
        {
            try
            {
                var books = await _bookRepository.GetBooksByCategoryAsync(categoryId);
                if (books == null || !books.Any())
                    return NotFound(new { Message = "No books found for this category" });

                var dtos = books.Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    AvailableCopies = b.AvailableCopies,
                    CategoryId = b.CategoryId,
                });

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching books by category", Details = ex.Message });
            }
        }

        // GET: api/book/search?q=keyword
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDTO>>> Search([FromQuery] string q)
        {
            try
            {
                var results = string.IsNullOrWhiteSpace(q)
                    ? await _bookRepository.GetAllAsync()
                    : await _bookRepository.SearchBooksAsync(q);

                var dtos = results.Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    AvailableCopies = b.AvailableCopies,
                    CategoryId  = b.CategoryId
                    
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error searching books", Details = ex.Message });
            }
        }

        // POST: api/book
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDTO>> Create([FromBody] BookDTO dto)
        {
            try
            {
                if (dto == null) return BadRequest(new { Message = "Invalid book data" });

                var cat = await _categoryRepository.GetByIdAsync(dto.CategoryId);
                if (cat == null)
                    return BadRequest(new { Message = "Invalid category" });

                var book = new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    ISBN = "AUTO-" + dto.Title.Substring(0, 3),
                    AvailableCopies = dto.AvailableCopies,
                    CategoryId = cat.CategoryId
                };

                await _bookRepository.AddAsync(book);
                dto.BookId = book.BookId;

                return CreatedAtAction(nameof(GetById), new { id = book.BookId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating book", Details = ex.Message });
            }
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDTO dto)
        {
            try
            {
                var existing = await _bookRepository.GetByIdAsync(id);
                if (existing == null) return NotFound(new { Message = "Book not found" });

                existing.Title = dto.Title;
                existing.Author = dto.Author;
                existing.AvailableCopies = dto.AvailableCopies;

                await _bookRepository.UpdateAsync(existing);
                return Ok(new { Message = "Book updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating book", Details = ex.Message });
            }
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _bookRepository.GetByIdAsync(id);
                if (existing == null) return NotFound(new { Message = "Book not found" });

                await _bookRepository.DeleteAsync(id);
                return Ok(new { Message = "Book deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting book", Details = ex.Message });
            }
        }
    }
}

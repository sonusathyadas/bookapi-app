using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using System.Threading.Tasks;
using BookAPI.DTOs;
using BookAPI.Models;
using BookAPI.Repositories;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private const string InternalServerErrorMessage = "Internal server error";

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>A list of books as DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                var dtoList = books.Select(MapToDto).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.Error.WriteLine($"Exception in GetBooks: {ex}");
                return StatusCode(500, InternalServerErrorMessage);
            }
        }

        /// <summary>
        /// Searches books by the provided searchText (query string).
        /// Example: GET /api/books/search?searchText=history
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] string searchText)
        {
            try
            {
                var books = await _bookRepository.SearchBooksAsync(searchText ?? string.Empty);
                var dtoList = books.Select(MapToDto).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.Error.WriteLine($"Exception in SearchBooks: {ex}");
                return StatusCode(500, InternalServerErrorMessage);
            }
        }

        /// <summary>
        /// Retrieves a specific book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book as a DTO, or a 404 status if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(MapToDto(book));
            }
            catch (Exception ex)
            {
                // Log the exception details for observability
                Console.Error.WriteLine($"Exception in GetBook: {ex}");
                return StatusCode(500, InternalServerErrorMessage);
            }
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="createBookDto">The DTO containing the details of the book to create.</param>
        /// <returns>The created book as a DTO, or a 400 status if the input is invalid.</returns>
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            try
            {
                if (createBookDto == null)
                {
                    return BadRequest();
                }

                var book = new Book
                {
                    Title = createBookDto.Title,
                    Author = createBookDto.Author,
                    ImageUrl = createBookDto.ImageUrl ?? string.Empty,
                    Language = createBookDto.Language,
                    Category = createBookDto.Category,
                    PublishedYear = createBookDto.PublishedYear
                };

                await _bookRepository.CreateBookAsync(book);
                var bookDto = MapToDto(book);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="updateBookDto">The DTO containing the updated details of the book.</param>
        /// <returns>A 204 status if the update is successful, or a 404 status if the book is not found.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                var existing = await _bookRepository.GetBookByIdAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }

                existing.Title = updateBookDto.Title;
                existing.Author = updateBookDto.Author;
                existing.ImageUrl = updateBookDto.ImageUrl;
                existing.Language = updateBookDto.Language;
                existing.Category = updateBookDto.Category;
                existing.PublishedYear = updateBookDto.PublishedYear;

                await _bookRepository.UpdateBookAsync(existing);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>A 204 status if the deletion is successful, or a 404 status if the book is not found.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                var existing = await _bookRepository.GetBookByIdAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }
                await _bookRepository.DeleteBookAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }


        // add a api method that insert a book using sql raw query to demonstrate the use of sql injection prevention        /// <summary>
        /// Inserts a new book using a raw SQL query to demonstrate SQL injection
        
        [HttpPost("raw-insert")]
        public async Task<ActionResult<BookDto>> InsertBookRawSql(CreateBookDto createBookDto)
        {
            try
            {
                if (createBookDto == null)
                {
                    return BadRequest();
                }

                var book = new Book
                {
                    Title = createBookDto.Title,
                    Author = createBookDto.Author,
                    ImageUrl = createBookDto.ImageUrl ?? string.Empty,
                    Language = createBookDto.Language,
                    Category = createBookDto.Category,
                    PublishedYear = createBookDto.PublishedYear
                };

                await _bookRepository.InsertBookRawSqlAsync(book);
                var bookDto = MapToDto(book);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Maps a Book entity to a BookDto.
        /// </summary>
        /// <param name="book">The book entity to map.</param>
        /// <returns>The mapped BookDto.</returns>
        private static BookDto MapToDto(Book book) => new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ImageUrl = book.ImageUrl,
            Language = book.Language,
            Category = book.Category,
            PublishedYear = book.PublishedYear
        };
    }
}
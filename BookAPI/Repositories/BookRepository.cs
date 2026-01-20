using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using BookAPI.Data;
using System.Linq; // added for LINQ extension methods

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }
        public async Task CreateBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        // new: search implementation
        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchText)
        {
            var normalized = (searchText ?? string.Empty).Trim().ToLower();
            if (string.IsNullOrEmpty(normalized))
            {
                return await GetAllBooksAsync();
            }

            return await _context.Books
                .Where(b =>
                    ((b.Title ?? string.Empty).ToLower().Contains(normalized)) ||
                    ((b.Author ?? string.Empty).ToLower().Contains(normalized)) ||
                    ((b.Category ?? string.Empty).ToLower().Contains(normalized)) ||
                    ((b.Language ?? string.Empty).ToLower().Contains(normalized))
                )
                .ToListAsync();
        }

        // Raw SQL insert - demonstrates SQL injection vulnerability
        public async Task InsertBookRawSqlAsync(Book book)
        {
            var sql = $"INSERT INTO Books (Title, Author, ImageUrl, Language, Category, PublishedYear) " +
                      $"VALUES ('{book.Title}', '{book.Author}', '{book.ImageUrl}', '{book.Language}', '{book.Category}', {book.PublishedYear})";
            
            await _context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints for User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "To Kill a Mockingbird", Author = "Harper Lee", ImageUrl = "https://covers.openlibrary.org/b/id/8225261-L.jpg", Language = "English", Category = "Fiction", PublishedYear = 1960 },
                new Book { Id = 2, Title = "1984", Author = "George Orwell", ImageUrl = "https://covers.openlibrary.org/b/id/7222246-L.jpg", Language = "English", Category = "Dystopian", PublishedYear = 1949 },
                new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ImageUrl = "https://covers.openlibrary.org/b/id/7352160-L.jpg", Language = "English", Category = "Classic", PublishedYear = 1925 },
                new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", ImageUrl = "https://covers.openlibrary.org/b/id/8091016-L.jpg", Language = "English", Category = "Romance", PublishedYear = 1813 },
                new Book { Id = 5, Title = "The Hobbit", Author = "J.R.R. Tolkien", ImageUrl = "https://covers.openlibrary.org/b/id/8101346-L.jpg", Language = "English", Category = "Fantasy", PublishedYear = 1937 },
                new Book { Id = 6, Title = "The Catcher in the Rye", Author = "J.D. Salinger", ImageUrl = "https://covers.openlibrary.org/b/id/8231856-L.jpg", Language = "English", Category = "Fiction", PublishedYear = 1951 },
                new Book { Id = 7, Title = "Moby-Dick", Author = "Herman Melville", ImageUrl = "https://covers.openlibrary.org/b/id/8100923-L.jpg", Language = "English", Category = "Adventure", PublishedYear = 1851 },
                new Book { Id = 8, Title = "War and Peace", Author = "Leo Tolstoy", ImageUrl = "https://covers.openlibrary.org/b/id/8231852-L.jpg", Language = "Russian", Category = "Historical", PublishedYear = 1869 },
                new Book { Id = 9, Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", ImageUrl = "https://covers.openlibrary.org/b/id/8235116-L.jpg", Language = "English", Category = "Fantasy", PublishedYear = 1954 },
                new Book { Id = 10, Title = "Crime and Punishment", Author = "Fyodor Dostoevsky", ImageUrl = "https://covers.openlibrary.org/b/id/8319781-L.jpg", Language = "Russian", Category = "Psychological", PublishedYear = 1866 }
            );

        }
    }
}
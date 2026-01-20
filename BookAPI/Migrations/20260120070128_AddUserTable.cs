using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    PublishedYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Mobile = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Category", "ImageUrl", "Language", "PublishedYear", "Title" },
                values: new object[,]
                {
                    { 1, "Harper Lee", "Fiction", "https://covers.openlibrary.org/b/id/8225261-L.jpg", "English", 1960, "To Kill a Mockingbird" },
                    { 2, "George Orwell", "Dystopian", "https://covers.openlibrary.org/b/id/7222246-L.jpg", "English", 1949, "1984" },
                    { 3, "F. Scott Fitzgerald", "Classic", "https://covers.openlibrary.org/b/id/7352160-L.jpg", "English", 1925, "The Great Gatsby" },
                    { 4, "Jane Austen", "Romance", "https://covers.openlibrary.org/b/id/8091016-L.jpg", "English", 1813, "Pride and Prejudice" },
                    { 5, "J.R.R. Tolkien", "Fantasy", "https://covers.openlibrary.org/b/id/8101346-L.jpg", "English", 1937, "The Hobbit" },
                    { 6, "J.D. Salinger", "Fiction", "https://covers.openlibrary.org/b/id/8231856-L.jpg", "English", 1951, "The Catcher in the Rye" },
                    { 7, "Herman Melville", "Adventure", "https://covers.openlibrary.org/b/id/8100923-L.jpg", "English", 1851, "Moby-Dick" },
                    { 8, "Leo Tolstoy", "Historical", "https://covers.openlibrary.org/b/id/8231852-L.jpg", "Russian", 1869, "War and Peace" },
                    { 9, "J.R.R. Tolkien", "Fantasy", "https://covers.openlibrary.org/b/id/8235116-L.jpg", "English", 1954, "The Lord of the Rings" },
                    { 10, "Fyodor Dostoevsky", "Psychological", "https://covers.openlibrary.org/b/id/8319781-L.jpg", "Russian", 1866, "Crime and Punishment" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

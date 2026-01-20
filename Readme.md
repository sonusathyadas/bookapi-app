# BookAPI

## Description
BookAPI is a RESTful API for managing a collection of books. It allows users to perform CRUD operations on books, including retrieving all books, retrieving a specific book by ID, creating new books, updating existing books, and deleting books.

## Project Structure
```
BookAPI/
├── Controllers/
│   └── BooksController.cs       # Handles HTTP requests and responses
├── Data/
│   └── AppDbContext.cs          # Database context for EF Core
├── DTOs/
│   ├── BookDto.cs               # Data Transfer Object for books
│   ├── CreateBookDto.cs         # DTO for creating books
│   └── UpdateBookDto.cs         # DTO for updating books
├── Models/
│   └── Book.cs                  # Book entity model
├── Repositories/
│   ├── IBookRepository.cs       # Interface for book repository
│   └── BookRepository.cs        # Implementation of book repository
└── Program.cs                   # Entry point of the application
```

## Patterns and Practices
- **Repository Pattern**: Used to abstract data access logic and promote separation of concerns.
- **DTOs (Data Transfer Objects)**: Used to decouple the API layer from the data layer and ensure only necessary data is exposed.
- **Dependency Injection**: Used to inject the repository into the controller for better testability and maintainability.
- **Entity Framework Core**: Used as the ORM for database operations.

## Classes and Descriptions
- **Book**: Represents the book entity with properties like `Id`, `Title`, `Author`, `ImageUrl`, `Language`, `Category`, and `PublishedYear`.
- **BookDto**: DTO for exposing book data in API responses.
- **CreateBookDto**: DTO for creating new books.
- **UpdateBookDto**: DTO for updating existing books.
- **AppDbContext**: Database context for managing EF Core operations.
- **IBookRepository**: Interface defining the contract for book repository operations.
- **BookRepository**: Implementation of `IBookRepository` for data access.
- **BooksController**: API controller for handling HTTP requests related to books.

## API Endpoints
| HTTP Method | Endpoint         | Description                          |
|-------------|------------------|--------------------------------------|
| GET         | /api/books       | Retrieves all books                  |
| GET         | /api/books/{id}  | Retrieves a specific book by ID      |
| POST        | /api/books       | Creates a new book                   |
| PUT         | /api/books/{id}  | Updates an existing book by ID       |
| DELETE      | /api/books/{id}  | Deletes a book by ID                 |

## Steps to Build, Test, Run, and Publish

### Build
1. Open a terminal or command prompt.
2. Navigate to the project directory.
3. Run the following command to build the project:
   ```bash
   dotnet build
   ```

### Test
1. Ensure you have unit tests written for the project.
2. Run the following command to execute the tests:
   ```bash
   dotnet test
   ```

### Run
1. Run the following command to start the application:
   ```bash
   dotnet run
   ```
2. The API will be available at `http://localhost:<port>`.

### Publish
1. Run the following command to publish the project:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
2. The published files will be available in the `./publish` directory.

## Notes
- Ensure you have the required database configured and the connection string set in `appsettings.json`.
- Use tools like Postman or Swagger to test the API endpoints.
- Follow best practices for exception handling and logging in production environments.

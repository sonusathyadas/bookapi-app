# BookAPI

## Description
BookAPI is a RESTful API for managing a collection of books with JWT authentication. It allows users to perform CRUD operations on books, including retrieving all books, retrieving a specific book by ID, creating new books, updating existing books, and deleting books. The API includes user authentication with JWT tokens for secure access.

## Project Structure
```
BookAPI/
├── Controllers/
│   ├── AuthController.cs         # Handles authentication requests (login, register)
│   └── BooksController.cs        # Handles HTTP requests and responses for books
├── Data/
│   └── AppDbContext.cs           # Database context for EF Core
├── DTOs/
│   ├── AuthResponseDto.cs        # DTO for authentication responses
│   ├── BookDto.cs                # Data Transfer Object for books
│   ├── CreateBookDto.cs          # DTO for creating books
│   ├── LoginDto.cs               # DTO for user login
│   ├── RegisterDto.cs            # DTO for user registration
│   └── UpdateBookDto.cs          # DTO for updating books
├── Models/
│   ├── Book.cs                   # Book entity model
│   └── User.cs                   # User entity model
├── Repositories/
│   ├── IBookRepository.cs        # Interface for book repository
│   ├── BookRepository.cs         # Implementation of book repository
│   ├── IUserRepository.cs        # Interface for user repository
│   └── UserRepository.cs         # Implementation of user repository
└── Program.cs                    # Entry point of the application
```

## Patterns and Practices
- **Repository Pattern**: Used to abstract data access logic and promote separation of concerns.
- **DTOs (Data Transfer Objects)**: Used to decouple the API layer from the data layer and ensure only necessary data is exposed.
- **Dependency Injection**: Used to inject the repository into the controller for better testability and maintainability.
- **Entity Framework Core**: Used as the ORM for database operations.
- **JWT Authentication**: Used for secure user authentication and authorization.
- **Password Hashing**: BCrypt is used to securely hash user passwords.

## Classes and Descriptions
- **Book**: Represents the book entity with properties like `Id`, `Title`, `Author`, `ImageUrl`, `Language`, `Category`, and `PublishedYear`.
- **User**: Represents the user entity with properties like `Id`, `Username`, `Password`, `Email`, and `Mobile`.
- **BookDto**: DTO for exposing book data in API responses.
- **CreateBookDto**: DTO for creating new books.
- **UpdateBookDto**: DTO for updating existing books.
- **LoginDto**: DTO for user login credentials.
- **RegisterDto**: DTO for user registration.
- **AuthResponseDto**: DTO for authentication responses containing JWT token and user info.
- **AppDbContext**: Database context for managing EF Core operations.
- **IBookRepository**: Interface defining the contract for book repository operations.
- **BookRepository**: Implementation of `IBookRepository` for data access.
- **IUserRepository**: Interface defining the contract for user repository operations.
- **UserRepository**: Implementation of `IUserRepository` for data access.
- **BooksController**: API controller for handling HTTP requests related to books.
- **AuthController**: API controller for handling authentication requests.

## API Endpoints

### Authentication Endpoints
| HTTP Method | Endpoint            | Description                          |
|-------------|---------------------|--------------------------------------|
| POST        | /api/auth/register  | Register a new user                  |
| POST        | /api/auth/login     | Login and receive JWT token          |

### Book Endpoints
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

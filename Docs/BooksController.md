# Books API (BooksController)

Base route: `/api/books`  
Replace `{host}` with your server (e.g. `https://localhost:5001`).

## Summary
This document describes the Books API endpoints implemented in BooksController.cs. It uses the following DTOs:

- BookDto
  - Id: int
  - Title: string
  - Author: string
  - ImageUrl: string
  - Language: string
  - Category: string
  - PublishedYear: int

- CreateBookDto
  - Title: string (required)
  - Author: string (required)
  - ImageUrl: string (optional)
  - Language: string
  - Category: string
  - PublishedYear: int

- UpdateBookDto
  - Title: string (required)
  - Author: string (required)
  - ImageUrl: string (optional)
  - Language: string
  - Category: string
  - PublishedYear: int

Common response codes:
- 200 OK — Successful GET returning data.
- 201 Created — POST created a new resource (Location header set).
- 204 No Content — Successful update or delete with no body.
- 400 Bad Request — Invalid input/payload.
- 404 Not Found — Resource not found.
- 500 Internal Server Error — Server error.

---

## GET /api/books
Description: Retrieve all books.

Request:
- Method: GET
- URL: {host}/api/books
- Headers: Accept: application/json

Response (200):
[
  {
    "id": 1,
    "title": "Example Title",
    "author": "Author Name",
    "imageUrl": "http://...",
    "language": "English",
    "category": "Fiction",
    "publishedYear": 2020
  },
  ...
]

Example - cURL:
curl -sS -X GET "{host}/api/books" -H "Accept: application/json"

Example - axios:
```javascript

const res = await axios.get("{host}/api/books");
console.log(res.data);
```
---

## GET /api/books/{id}
Description: Retrieve a single book by id.

Request:
- Method: GET
- URL: {host}/api/books/123

Response:
- 200 OK: BookDto
- 404 Not Found: no body

Example - cURL:
curl -sS -X GET "{host}/api/books/1" -H "Accept: application/json"

Example - axios:
try {
  const res = await axios.get("{host}/api/books/1");
  console.log(res.data);
} catch (err) {
  if (err.response && err.response.status === 404) {
    console.log("Book not found");
  }
}

---

## POST /api/books
Description: Create a new book.

Request:
- Method: POST
- URL: {host}/api/books
- Headers:
  - Content-Type: application/json
- Body (CreateBookDto):
{
  "title": "New Book",
  "author": "Author Name",
  "imageUrl": "http://...",
  "language": "English",
  "category": "Non-fiction",
  "publishedYear": 2023
}

Response:
- 201 Created
  - Location header: URL to GET the created book (e.g. /api/books/{id})
  - Body: BookDto of created book (including assigned Id)
- 400 Bad Request: if payload is null/invalid
- 500 Internal Server Error

Example - cURL:
curl -sS -X POST "{host}/api/books" \
  -H "Content-Type: application/json" \
  -d '{
    "title":"New Book",
    "author":"Author Name",
    "imageUrl":"",
    "language":"English",
    "category":"Non-fiction",
    "publishedYear":2023
  }'

Example - axios:
const payload = {
  title: "New Book",
  author: "Author Name",
  imageUrl: "",
  language: "English",
  category: "Non-fiction",
  publishedYear: 2023
};
const res = await axios.post("{host}/api/books", payload);
console.log(res.status, res.headers.location, res.data);

---

## PUT /api/books/{id}
Description: Update an existing book.

Request:
- Method: PUT
- URL: {host}/api/books/1
- Headers: Content-Type: application/json
- Body (UpdateBookDto): same shape as CreateBookDto

Response:
- 204 No Content: update succeeded
- 404 Not Found: book id does not exist
- 500 Internal Server Error

Example - cURL:
curl -sS -X PUT "{host}/api/books/1" \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Updated Title",
    "author":"Updated Author",
    "imageUrl":"http://...",
    "language":"English",
    "category":"Fiction",
    "publishedYear":2024
  }'

Example - axios:
await axios.put("{host}/api/books/1", {
  title: "Updated Title",
  author: "Updated Author",
  imageUrl: "http://...",
  language: "English",
  category: "Fiction",
  publishedYear: 2024
});

---

## DELETE /api/books/{id}
Description: Delete a book by id.

Request:
- Method: DELETE
- URL: {host}/api/books/1

Response:
- 204 No Content: deleted
- 404 Not Found: id not found
- 500 Internal Server Error

Example - cURL:
curl -sS -X DELETE "{host}/api/books/1"

Example - axios:
await axios.delete("{host}/api/books/1");

---

## Errors & Logging
- The controller returns generic 500 responses on exceptions. For debugging, check server logs (exceptions are written to console in the current implementation).
- Validate input before calling the API to reduce invalid requests.

---

## Notes
- Ensure your API server is running and the base URL uses correct protocol and port.
- Authentication/authorization are not described in the current controller — if present in the app, include necessary headers (e.g., Authorization).
- Adjust examples to match your environment (host, ports, etc.).

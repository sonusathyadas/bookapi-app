using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BookAPI.Controllers;
using BookAPI.Repositories;
using BookAPI.Models;
using BookAPI.DTOs;

namespace BookAPI.Tests.Controllers
{
    public class BooksControllerTests
    {
        [Fact]
        public async Task GetBooks_ReturnsOkWithList()
        {
            var mockRepo = new Mock<IBookRepository>();
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "T1", Author = "A1", ImageUrl = "", Language = "EN", Category = "C", PublishedYear = 2020 }
            };
            mockRepo.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);

            var controller = new BooksController(mockRepo.Object);
            var actionResult = await controller.GetBooks();

            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtoList = Assert.IsType<List<BookDto>>(ok.Value);
            Assert.Single(dtoList);
            Assert.Equal("T1", dtoList[0].Title);
        }

        [Fact]
        public async Task GetBook_ReturnsOk_WhenFound()
        {
            var mockRepo = new Mock<IBookRepository>();
            var book = new Book { Id = 2, Title = "T2", Author = "A2", ImageUrl = "", Language = "EN", Category = "C2", PublishedYear = 2021 };
            mockRepo.Setup(r => r.GetBookByIdAsync(2)).ReturnsAsync(book);

            var controller = new BooksController(mockRepo.Object);
            var actionResult = await controller.GetBook(2);

            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<BookDto>(ok.Value);
            Assert.Equal(2, dto.Id);
            Assert.Equal("T2", dto.Title);
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_WhenMissing()
        {
            var mockRepo = new Mock<IBookRepository>();
            mockRepo.Setup(r => r.GetBookByIdAsync(99)).ReturnsAsync((Book?)null);

            var controller = new BooksController(mockRepo.Object);
            var actionResult = await controller.GetBook(99);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtAction()
        {
            var mockRepo = new Mock<IBookRepository>();
            mockRepo.Setup(r => r.CreateBookAsync(It.IsAny<Book>()))
                .Returns(Task.CompletedTask)
                .Callback<Book>(b => b.Id = 42);

            var controller = new BooksController(mockRepo.Object);
            var createDto = new CreateBookDto
            {
                Title = "New",
                Author = "Auth",
                ImageUrl = null,
                Language = "EN",
                Category = "Cat",
                PublishedYear = 2022
            };

            var actionResult = await controller.CreateBook(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var dto = Assert.IsType<BookDto>(created.Value);
            Assert.Equal(42, ((dynamic)created.RouteValues!)["id"]);
            Assert.Equal("New", dto.Title);
            mockRepo.Verify(r => r.CreateBookAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent_WhenFound()
        {
            var mockRepo = new Mock<IBookRepository>();
            var existing = new Book { Id = 3, Title = "Old", Author = "A", ImageUrl = "", Language = "EN", Category = "C", PublishedYear = 2019 };
            mockRepo.Setup(r => r.GetBookByIdAsync(3)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.UpdateBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var controller = new BooksController(mockRepo.Object);
            var updateDto = new UpdateBookDto
            {
                Title = "Updated",
                Author = "A2",
                ImageUrl = "img.png",
                Language = "FR",
                Category = "C2",
                PublishedYear = 2023
            };

            var result = await controller.UpdateBook(3, updateDto);
            Assert.IsType<NoContentResult>(result);
            mockRepo.Verify(r => r.UpdateBookAsync(It.Is<Book>(b => b.Id == 3 && b.Title == "Updated" && b.Language == "FR")), Times.Once);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFound_WhenMissing()
        {
            var mockRepo = new Mock<IBookRepository>();
            mockRepo.Setup(r => r.GetBookByIdAsync(999)).ReturnsAsync((Book?)null);

            var controller = new BooksController(mockRepo.Object);
            var updateDto = new UpdateBookDto
            {
                Title = "X",
                Author = "Y",
                ImageUrl = null,
                Language = "EN",
                Category = "C",
                PublishedYear = 2000
            };

            var result = await controller.UpdateBook(999, updateDto);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenFound()
        {
            var mockRepo = new Mock<IBookRepository>();
            var existing = new Book { Id = 4, Title = "D", Author = "A", ImageUrl = "", Language = "EN", Category = "C", PublishedYear = 2018 };
            mockRepo.Setup(r => r.GetBookByIdAsync(4)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.DeleteBookAsync(4)).Returns(Task.CompletedTask);

            var controller = new BooksController(mockRepo.Object);
            var result = await controller.DeleteBook(4);
            Assert.IsType<NoContentResult>(result);
            mockRepo.Verify(r => r.DeleteBookAsync(4), Times.Once);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenMissing()
        {
            var mockRepo = new Mock<IBookRepository>();
            mockRepo.Setup(r => r.GetBookByIdAsync(12345)).ReturnsAsync((Book?)null);

            var controller = new BooksController(mockRepo.Object);
            var result = await controller.DeleteBook(12345);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
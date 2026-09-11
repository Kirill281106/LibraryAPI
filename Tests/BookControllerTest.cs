using LibraryAPI.Controllers;
using LibraryAPI.DTOs.AuthorDTO;
using LibraryAPI.DTOs.BookDTO;
using LibraryAPI.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _bookController;
        private List<BookResponce> expectedBooks;
        public BookControllerTest()
        {
            _mockBookService = new Mock<IBookService>();
            _bookController = new BookController(_mockBookService.Object);
        }

        public List<BookResponce> Books()
        {
            return new List<BookResponce>
            {
                new BookResponce { Id = 1, Title = "Война и мир", Year = 1869 },
                new BookResponce { Id = 2, Title = "Анна Каренина", Year = 1877 }
            };
        }
        private BookWithAuthor BookAndAuthor(int id)
        {
            expectedBooks = Books();
            var book = expectedBooks.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return null;

            return new BookWithAuthor
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Genre = "Роман",
                Author = new AuthorResponce
                {
                    Id = 1,
                    FullName = "Лев Толстой",
                    BirthDate = new DateTime(1828, 9, 9),
                    Country = "Россия"
                }
            };
        }

        [Fact]
        public async Task GetAllBooks_Test()
        {
            // Arrange
            var expectedBooks = Books();
            _mockBookService.Setup(s => s.GetAllBooks())
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookController.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var books = Assert.IsType<List<BookResponce>>(okResult.Value);
            Assert.Equal(2, books.Count);
            Assert.Equal("Война и мир", books[0].Title);
        }
        [Fact]
        public async Task GetAllBooksIsNull_Test()
        {
            // Arrange
            var expectedBooks = new List<BookResponce>();
            _mockBookService.Setup(s => s.GetAllBooks()).ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookController.GetBooks();

            //Assert
            var resultIsNull = Assert.IsType<OkObjectResult>(result);
            var books = Assert.IsType<List<BookResponce>>(resultIsNull.Value);
            Assert.Empty(books);
        }
        [Fact]
        public async Task GetBookById_Test()
        {
            int id = 2;
            var expectedBook = BookAndAuthor(id);


            _mockBookService.Setup(s => s.GetBookById(id))
                .ReturnsAsync(expectedBook);

            var result = await _bookController.GetBookById(2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var book = Assert.IsType<BookWithAuthor>(okResult.Value);
            Assert.Equal(id, book.Id);
            Assert.Equal("Анна Каренина", book.Title);
            Assert.Equal(1877, book.Year);
        }
        [Fact]
        public async Task PostBook_Test()
        {
            var createBookDTO = new CreateBookDTO
            {
                Title = "Детство",
                Year = 1852,
                Genre = "Повессть",
                AuthorId = 1
            };
            var expectedBook = new BookResponce
            {
                Id = 3,
                Title = "Детство",
                Year = 1852,
                Genre = "Повессть",
                AuthorId = 1
            };
            _mockBookService.Setup(s => s.CreateBook(createBookDTO))
        .ReturnsAsync(expectedBook);

            var result = await _bookController.CreateNewBook(createBookDTO);

            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var book = Assert.IsType<BookResponce>(okResult.Value);

            Assert.Equal("Детство", book.Title);
            Assert.Equal(1, book.AuthorId);
        }
        [Fact]
        public async Task UpdateBook_Test()
        {
            int id = 1;
            var book = new UpdateBookDTO
            {
                Title = "Война и мир (обновлено)",
                Year = 1870,
                Genre = "Роман",
                AuthorId = 1
            };

            _mockBookService.Setup(s => s.UpdateBook(id, book)).ReturnsAsync(true);
            var result = await _bookController.UpdateBook(id, book);

            var okResult = Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task UpdateBook_WhenNotFound_Test()
        {
            int id = 123;
            var book = new UpdateBookDTO
            {
                Title = "Война и мир (обновлено)",
                Year = 1870,
                Genre = "Роман",
                AuthorId = 1
            };

            _mockBookService.Setup(s => s.UpdateBook(id, book)).ReturnsAsync(false);
            var result = await _bookController.UpdateBook(id, book);

            var okResult = Assert.IsType<NotFoundResult>(result);
        }
        // удаление проверил
        [Fact]
        public async Task DeleteBook_Test()
        {
            int id = 111;
            _mockBookService.Setup(s => s.DeleteBook(id)).ReturnsAsync(true);

            var result1 = await _bookController.DeleteBook(id);

            var okResult = Assert.IsType<NoContentResult>(result1);
        }
        [Fact]
        public async Task DeleteBook_WhenNotFound_Test()
        {
            int id = 111;

            _mockBookService.Setup(s => s.DeleteBook(id)).ReturnsAsync(false);

            var result2 = await _bookController.DeleteBook(id);

            var NotResult = Assert.IsType<NotFoundResult>(result2);
        }
    }
}
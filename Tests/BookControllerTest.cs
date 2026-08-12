using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using LibraryAPI.Controllers;
using LibraryAPI.DTOs.AuthorDTO;
using LibraryAPI.DTOs.BookDTO;
using LibraryAPI.IService.Interfaces;

namespace LibraryAPI.Tests
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _bookController;
        private readonly List<BookResponce> expectedBooks;
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

        private BookWithAuthor BookAndAuthor(int id)
        {
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
        public async Task GetBookById_Test()
        {
            int id = 2;
            var expectedBook = BookAndAuthor(id);
            _mockBookService.Setup(s => s.GetBookById(id))
                .ReturnsAsync(expectedBook);

            var result = await _bookController.GetBookById(2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var book = Assert.IsType<BookResponce>(okResult.Value);
            Assert.Equal(id, book.Id);
            Assert.Equal("Анна Каренина", book.Title);
            Assert.Equal(1877, book.Year);
        }
    }
}
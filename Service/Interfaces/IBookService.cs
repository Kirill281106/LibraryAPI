using LibraryAPI.DTOs.BookDTO;

namespace LibraryAPI.IService.Interfaces
{
    public interface IBookService
    {
        public Task<List<BookResponce>> GetAllBooks();
        public Task<BookWithAuthor?> GetBookById(int id);
        public Task<BookResponce> CreateBook(CreateBookDTO ctreateBookDTO);
        public Task<bool> UpdateBook(int id, UpdateBookDTO updateBookDTO);
        public Task<bool> DeleteBook(int id);
    }
}

using Microsoft.EntityFrameworkCore;
using LibraryAPI.DB;
using LibraryAPI.DTOs.AuthorDTO;
using LibraryAPI.DTOs.BookDTO;
using LibraryAPI.IService.Interfaces;
using LibraryAPI.Models;

namespace LibraryAPI.Service.Classes
{
    class BookService:IBookService
    {
        AppDbContext _context;
        public BookService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<List<BookResponce>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            var booksResponce = books.Select(b => new BookResponce
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Genre = b.Genre,
                AuthorId = b.AuthorId
            }).ToList();
            return booksResponce;
        }
        public async Task<BookWithAuthor?> GetBookById(int id)
        {
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id==id);
            return new BookWithAuthor
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Genre = book.Genre,
                Author = new AuthorResponce
                {
                    Id = book.Author.Id,
                    FullName = book.Author.FullName,
                    BirthDate = book.Author.BirthDate,
                    Country = book.Author.Country,
                }
            };
        }
        public async Task<BookResponce> CreateBook(CreateBookDTO createBookDTO)
        {
            var authors = await _context.Authors.AnyAsync(a => a.Id == createBookDTO.AuthorId);

            if (!authors)
                throw new ArgumentException($"Автор с таким id: {createBookDTO.AuthorId} не существует");
            var book = new Book
            {
                Title = createBookDTO.Title,
                Year = createBookDTO.Year,
                Genre = createBookDTO.Genre,
                AuthorId = createBookDTO.AuthorId
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            var bookResponce = new BookResponce
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Genre = book.Genre,
                AuthorId = book.AuthorId
            };
            return bookResponce;
        }
        public async Task<bool> UpdateBook(int id, UpdateBookDTO updateBookDTO)
        {
            var bookUpdated =await _context.Books.FindAsync(id);
            if(bookUpdated == null)
                return false;
            else
            {
                bookUpdated?.Title = updateBookDTO.Title;
                bookUpdated?.Year = updateBookDTO.Year;
                bookUpdated?.Genre = updateBookDTO.Genre;
                bookUpdated?.AuthorId = updateBookDTO.AuthorId;
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

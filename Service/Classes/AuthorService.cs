using Microsoft.EntityFrameworkCore;
using LibraryAPI.DB;
using LibraryAPI.Models;
using LibraryAPI.DTOs.AuthorDTO;
using LibraryAPI.IService.Interfaces;
using LibraryAPI.DTOs.BookDTO;
using System.Reflection;

namespace LibraryAPI.Service.Classes
{
    class AuthorService:IAuthorService
    {
        AppDbContext _context;
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AuthorResponce>> GetAllAuthors()
        {
            var autors = await _context.Authors.ToListAsync();
            var autorsResponce = autors.Select(a=>new AuthorResponce
            {
                Id = a.Id,
                FullName = a.FullName,
                BirthDate = a.BirthDate,
                Country = a.Country
            }).ToList();
            return autorsResponce; 
        }
        public async Task<AuthorWithBook?> GetAuthorById(int id)
        {
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            return new AuthorWithBook
            {
                Id = author.Id,
                FullName = author.FullName,
                BirthDate = author.BirthDate,
                Country = author.Country,
                BookThisAuthor = author.Books.Select(b => new BookResponce
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Genre = b.Genre,
                    AuthorId = b.AuthorId
                }).ToList()
            };
        }
        public async Task<AuthorResponce> CreateAuthor(CreateAuthorDTO createAuthorDTO)
        {
            var autorCreate = new Author
            {
                FullName = createAuthorDTO.FullName,
                BirthDate = createAuthorDTO.BirthDate,
                Country = createAuthorDTO.Country
            };
            await _context.Authors.AddAsync(autorCreate);
            await _context.SaveChangesAsync();
            var authorResponce = new AuthorResponce
            {
                Id = autorCreate.Id,
                FullName= autorCreate.FullName,
                BirthDate = autorCreate.BirthDate,
                Country = autorCreate.Country
            };
            return authorResponce;
        }
        public async Task<bool> UpdateAuthor(int id, UpdateAuthorDTO updateAuthorDTO)
        {
            var updatedAuthor = await _context.Authors.FindAsync(id);
            if(updatedAuthor == null) 
                return false;
            updatedAuthor.FullName = updateAuthorDTO.FullName;
            updatedAuthor.BirthDate = updateAuthorDTO.BirthDate;
            updatedAuthor.Country = updateAuthorDTO.Country;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if(author == null)
                return false;
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

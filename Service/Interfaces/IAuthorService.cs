using LibraryAPI.DTOs.AuthorDTO;

namespace LibraryAPI.IService.Interfaces
{
    public interface IAuthorService
    {
        public Task<List<AuthorResponce>> GetAllAuthors();
        public Task<AuthorWithBook?> GetAuthorById(int id);
        public Task<AuthorResponce> CreateAuthor(CreateAuthorDTO ctreateAuthorDTO);
        public Task<bool> UpdateAuthor(int id, UpdateAuthorDTO updateAuthorDTO);
        public Task<bool> DeleteAuthor (int id);
    }
}

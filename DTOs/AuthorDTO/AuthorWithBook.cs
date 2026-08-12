using LibraryAPI.DTOs.BookDTO;
namespace LibraryAPI.DTOs.AuthorDTO
{
    public class AuthorWithBook
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public List<BookResponce> BookThisAuthor { get; set; } = new List<BookResponce>();
    }
}

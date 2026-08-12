using LibraryAPI.DTOs.AuthorDTO;

namespace LibraryAPI.DTOs.BookDTO
{
    public class BookWithAuthor
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public AuthorResponce Author { get; set; } = null!;
    }
}

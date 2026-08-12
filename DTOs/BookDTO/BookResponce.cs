using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs.BookDTO
{
    public class BookResponce
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}

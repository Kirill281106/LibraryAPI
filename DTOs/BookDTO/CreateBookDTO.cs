namespace LibraryAPI.DTOs.BookDTO
{
    public class CreateBookDTO
    {
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int AuthorId { get; set; }

    }
}

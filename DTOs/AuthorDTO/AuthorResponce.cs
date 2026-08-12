namespace LibraryAPI.DTOs.AuthorDTO
{
    public class AuthorResponce
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Country {  get; set; } = string.Empty;
    }
}

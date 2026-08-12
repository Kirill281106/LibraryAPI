using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    internal class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = new List<Book>();
    }
}  

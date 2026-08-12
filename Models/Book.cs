using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    internal class Book
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Range(1000, 10000)]
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        [Required]
        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public Author Author {  get; set; } = null!;
    }
}

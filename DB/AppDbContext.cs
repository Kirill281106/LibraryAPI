using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;
namespace LibraryAPI.DB
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a=>a.Books).HasForeignKey(b => b.AuthorId);
        }
    }
}

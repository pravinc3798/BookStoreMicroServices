using Microsoft.EntityFrameworkCore;

namespace BookStore.Books.Entity;

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    {
    }

    public DbSet<BookEntity> Books { get; set; }
}

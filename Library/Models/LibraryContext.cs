using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Models
{
    public class LibraryContext : IdentityDbContext<LibraryUser>
    {
        public DbSet<Author> Authors {get; set;}
        public DbSet<Book> Books {get; set;}
        public DbSet<AuthorBook> AuthorBooks {get; set;}
        public DbSet<Checkout> Checkouts {get; set;}

        public LibraryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
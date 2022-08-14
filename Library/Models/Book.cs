using System.Collections.Generic;

namespace Library.Models
{
    public class Book
    {
        public Book()
        {
            this.AuthorBooks = new HashSet<AuthorBook>();
            this.Checkouts = new HashSet<Checkout>();
        }
        public int BookId {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public int Copies {get; set;}
        public virtual ICollection<AuthorBook> AuthorBooks {get; set;}
        public virtual ICollection<Checkout> Checkouts {get; set;}
    }
}
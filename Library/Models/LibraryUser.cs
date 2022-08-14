using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Library.Models
{
    public class LibraryUser :IdentityUser
    {
        public LibraryUser()
        {
            this.Checkouts = new HashSet<Checkout>();
        }
        public string FirstName {get; set;}
        public string LastName {get; set;}
        
        public string FullName {
            get {
                return FirstName + " " + LastName;
            }
        }
        public virtual ICollection<Checkout> Checkouts {get; set;}
    }
}
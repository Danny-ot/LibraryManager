using System;

namespace Library.Models
{
    public class Checkout
    {
        public int CheckoutId {get; set;}
        public int BookId {get; set;}
        public DateTime Date {get; set;}
        public bool Returned {get; set;}
        public virtual Book Book {get; set;}
        public virtual LibraryUser User {get; set;}

        public bool IsDue()
        {
          bool answer = false;
          int dateConfirm = DateTime.Now.CompareTo(this.Date.AddDays(7));
          if(dateConfirm > 0)
          {
            answer = true;
          }
          return answer; 
        }
    }
}
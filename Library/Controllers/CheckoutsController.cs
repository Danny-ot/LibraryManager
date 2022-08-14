using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace Library.Controllers
{
    [Authorize]
    public class CheckoutsController :Controller
    {
        private readonly LibraryContext _db;
        private readonly SignInManager<LibraryUser> _signInManager;
        private readonly UserManager<LibraryUser> _userManager;

        public CheckoutsController(LibraryContext db , SignInManager<LibraryUser> signInManager , UserManager<LibraryUser> userManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var model= _db.Checkouts.Include(checkout => checkout.Book).Where(entry => entry.User.Id == currentUser.Id).OrderByDescending(checkout => checkout.CheckoutId).ToList();
            ViewBag.ErrorMessage = "";
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(int bookId)
        {
            var book = _db.Books.FirstOrDefault(bk => bk.BookId == bookId);
            if(book.Copies == 0)
            {
                ViewBag.ErrorMessage = $"Sorry but {book.Name} is finished in this library";
                return RedirectToAction("Details" , "Books" , new {@id = bookId});
            }
            else
            {
                book.Copies = book.Copies - 1;
                _db.Entry(book).State = EntityState.Modified;
                _db.SaveChanges();
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUser = await _userManager.FindByIdAsync(userId);
                var checkout = new Checkout{BookId = bookId , User = currentUser , Date = DateTime.Now , Returned = false};
                _db.Checkouts.Add(checkout);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Liberian")]
        public IActionResult AllCheckouts()
        {
            ViewBag.ErrorMessage="";
            var model = _db.Checkouts.Include(checkout => checkout.Book).Include(checkout => checkout.User).OrderBy(checkout =>checkout.Returned).ToList();
            return View(model);
        }
        
        [Authorize(Roles = "Liberian")]
        public IActionResult ReturnBook(int checkoutId , int bookId)
        {
            var book = _db.Books.FirstOrDefault(bk => bk.BookId == bookId);
            book.Copies = book.Copies + 1;
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            var checkout = _db.Checkouts.FirstOrDefault(ck => ck.CheckoutId == checkoutId);
            checkout.Returned = true;
            _db.Entry(checkout).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("AllCheckouts");
        }

        public IActionResult Search(string userName)
        {
            var model = _db.Checkouts.Include(checkout => checkout.Book).Include(checkout => checkout.User).Where(ck => ck.User.FirstName.Contains(userName.ToLower()) || ck.User.LastName.Contains(userName.ToLower())).OrderBy(checkout =>checkout.Returned).ToList();
            return View("AllCheckouts" , model);
        }

        [Authorize(Roles = "Liberian")]
        public IActionResult Delete(int checkoutId)
        {
            var checkout = _db.Checkouts.FirstOrDefault(ck => ck.CheckoutId == checkoutId);
            if(checkout.Returned != true)
            { 
                ViewBag.ErrorMessage="You can't delete a book without returning it";             
                var model = _db.Checkouts.Include(checkout => checkout.Book).Include(checkout => checkout.User).OrderBy(checkout =>checkout.Returned).ToList();
                 return View("AllCheckouts" , model);
            }
            else
            {
                _db.Checkouts.Remove(checkout);
                _db.SaveChanges();
                return RedirectToAction("AllCheckouts");
            }
        }
    }
}
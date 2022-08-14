using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _db;

        public BooksController(LibraryContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Book> books= _db.Books.Include(books => books.AuthorBooks).ThenInclude(authorbook => authorbook.Author).ToList();
            return View(books);
        }

        [Authorize(Roles = "Liberian")]
        public IActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(_db.Authors , "AuthorId" , "Name");
            return View();
        }

        [Authorize(Roles = "Liberian")]
        [HttpPost]
        public IActionResult Create(Book book , int AuthorId)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            if(AuthorId != 0)
            {
                _db.AuthorBooks.Add(new AuthorBook{AuthorId = AuthorId , BookId = book.BookId});
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Book book = _db.Books.Include(bk => bk.AuthorBooks).ThenInclude(authbk => authbk.Author).FirstOrDefault(bk => bk.BookId == id);
            return View(book);
        }

        [Authorize(Roles = "Liberian")]
        public IActionResult Delete(int bookId)
        {
            Book book = _db.Books.FirstOrDefault(bk => bk.BookId == bookId);
            _db.Books.Remove(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Liberian")]
        public IActionResult Edit(int id)
        {
            Book book = _db.Books.FirstOrDefault(bk => bk.BookId == id);
            ViewBag.AuthorId = new SelectList(_db.Authors , "AuthorId" , "Name");
            return View(book);
        }

        [Authorize(Roles = "Liberian")]
        [HttpPost]
        public IActionResult Edit(Book book , int AuthorId)
        {
            var authorbook = _db.AuthorBooks.FirstOrDefault(authbk => authbk.AuthorId == AuthorId && authbk.BookId == book.BookId);
            if(authorbook == null)
            {
                if(AuthorId != 0)
                {
                    _db.AuthorBooks.Add(new AuthorBook {BookId = book.BookId , AuthorId = AuthorId});
                }
            }
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Liberian")]
        public IActionResult AddAuthor(int id)
        {
            var book = _db.Books.FirstOrDefault(bk => bk.BookId == id);
            ViewBag.AuthorId = new SelectList(_db.Authors , "AuthorId" , "Name");
            return View(book);
        }

        [Authorize(Roles = "Liberian")]
        [HttpPost]
        public IActionResult AddAuthor(Book book , int AuthorId)
        {
            if(AuthorId != 0)
            {
                _db.AuthorBooks.Add(new AuthorBook {BookId = book.BookId , AuthorId = AuthorId});
            }
            _db.SaveChanges();
            return RedirectToAction("Details" , new {id = book.BookId});
        }

        [Authorize(Roles ="Liberian")]
        [HttpPost]
        public IActionResult RemoveAuthor(int AuthorBookId)
        {
            var authbook = _db.AuthorBooks.FirstOrDefault(authb => authb.AuthorBookId == AuthorBookId);
            _db.AuthorBooks.Remove(authbook);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Search(string bookName)
        {
            List<Book> book = _db.Books.Where(bk => bk.Name.ToLower().Contains(bookName.ToLower())).ToList();
            return View("Index" , book);
        }

    }
}
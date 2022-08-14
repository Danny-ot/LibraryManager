using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize(Roles = "Liberian")]
    public class AuthorsController : Controller
    {
        private readonly LibraryContext _db;

        public AuthorsController(LibraryContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Author> authors = _db.Authors.ToList();
            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            _db.Authors.Add(author);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int authorId)
        {
            var author = _db.Authors.FirstOrDefault(auth => auth.AuthorId == authorId);
            _db.Authors.Remove(author);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
             var author = _db.Authors.Include(auth => auth.AuthorBooks).ThenInclude(authbk => authbk.Book).FirstOrDefault(auth => auth.AuthorId == id);
             return View(author);
        }

        public IActionResult Edit(int id)
        {
            var author = _db.Authors.FirstOrDefault(auth => auth.AuthorId == id);
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(Author author)
        {
            _db.Entry(author).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
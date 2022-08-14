using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Library.Models;
using System.Threading.Tasks;
using Library.ViewModels;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryContext _db;
        private readonly SignInManager<LibraryUser> _signInManager;
        private readonly UserManager<LibraryUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(LibraryContext db , SignInManager<LibraryUser> signInManager , UserManager<LibraryUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new LibraryUser {UserName = viewModel.Email , FirstName = viewModel.FirstName , LastName = viewModel.LastName};
                IdentityResult result = await _userManager.CreateAsync(user , viewModel.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user , "Patron");
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(viewModel.Email , viewModel.Password , isPersistent: true , lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index" , "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index" , "Home");
        }
    }
}
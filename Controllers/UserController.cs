using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class UserController : Controller
    {
        public enum AuthorizationState
        {
            Login,
            Register
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(User user)
        {
            await using var db = new LibraryDbContext();

            if (!ModelState.IsValid) { return View("Index", user); }

            User? userDetail = await db.Users.FirstOrDefaultAsync(
                x => x.UserName == user.UserName && x.Password == user.Password);

            if (ReferenceEquals(userDetail, null))
            {
                user.ErrorMessage = "Invalid credentials!";
                return View("Index", user);
            }

            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, userDetail);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string confirmPW)
        {
            await using var db = new LibraryDbContext();
            
            if (!ModelState.IsValid) { return View("Index", user); }
            
            if (!confirmPW.Equals(user.Password))
            {
                ModelState.AddModelError("Password", "The passwords are not the same");
                return View("Index", user);
            }

            if (await db.Users.AnyAsync(u => u.UserName.Equals(user.UserName)))
            {
                ModelState.AddModelError("UserName", $"The username \'{user.UserName}\' is already taken");
                return View("Index", user);
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, user);
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult SwitchPartialPage(AuthorizationState state)
        {
            HttpContext.Session.SetSessionObjectAsJson(Settings.LOGIN_PAGE_ID_KEY, (int)state);
            return View("Index");
        }
    }
}
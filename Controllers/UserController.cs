using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessionLogin = HttpContext.Session.GetSessionObjectFromJson<User>(Settings.SESSION_USER_KEY);

            if (ReferenceEquals(sessionLogin, null)) { return RedirectToAction("Login"); }

            await using var db = new LibraryDbContext();
            List<User> users = await db.Users.ToListAsync();
            return View(users);
        }

        [HttpGet] public ActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid) { return View(user); }

            await using var db = new LibraryDbContext();

            User? userDetail = await db.Users.FirstOrDefaultAsync(
                x => x.UserName == user.UserName && x.Password == user.Password);

            if (ReferenceEquals(userDetail, null))
            {
                user.ErrorMessage = "Invalid credentials!";
                return View(user);
            }

            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, userDetail);
            return RedirectToAction("Index");
        }

        [HttpGet] public ActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user, string confirmPW)
        {
            await using var db = new LibraryDbContext();

            if (!ModelState.IsValid) { return View(user); }

            if (!confirmPW.Equals(user.Password))
            {
                ModelState.AddModelError("Password", "The passwords are not the same");
                return View(user);
            }

            if (await db.Users.AnyAsync(u => u.UserName.Equals(user.UserName)))
            {
                ModelState.AddModelError("UserName", $"The username \'{user.UserName}\' is already taken");
                return View(user);
            }

            user.IsAdministrator = false;

            db.Users.Add(user);
            await db.SaveChangesAsync();
            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using var db = new LibraryDbContext();
            User? user = db.Users.FirstOrDefault(u => u.UserID == id);
            return View(user);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
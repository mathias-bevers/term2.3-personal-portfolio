using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(User user)
        {
            await using var context = new LibraryDbContext();

            if (!ModelState.IsValid) { return View("Index", user); }

            User? userDetail = await context.Users.FirstOrDefaultAsync(
                x => x.UserName == user.UserName && x.Password == user.Password);

            if (ReferenceEquals(userDetail, null))
            {
                user.ErrorMessage = "Invalid credentials!";
                return View("Index", user);
            }
            
            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, userDetail);
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
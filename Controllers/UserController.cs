using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Authorize(User user)
        {
            using var context = new LibraryDbContext();

            if (!ModelState.IsValid) { return View("Index", user); }
            
            User? userDetail = context.Users.FirstOrDefault(x => x.UserName == user.UserName
                                                                 && x.Password == user.Password);

            if (!ReferenceEquals(userDetail, null)) { return RedirectToAction("Index"); }

            user.ErrorMessage = "Invalid credentials!";
            return View("Index", user);
        }
    }
}
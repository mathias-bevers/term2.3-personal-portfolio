using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await using var db = new LibraryDbContext();
            List<Author> model = await db.Authors.AsNoTracking().ToListAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var user = HttpContext.Session.GetSessionObjectFromJson<User>(Settings.SESSION_USER_KEY);

            if (user is { IsAdministrator: true }) { return View(); }

            HttpContext.Items["ErrorMessage"] = "This page can only be accessed by administrators.";
            return StatusCode(StatusCodes.Status401Unauthorized);

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, LastName")] Author author)
        {
            await using var db = new LibraryDbContext();

            if (!ModelState.IsValid) { return View(author); }

            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
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

        [HttpGet] public IActionResult Create() => View();

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
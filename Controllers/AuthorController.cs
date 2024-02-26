using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var context = new LibraryDbContext())
            {
                List<Author> model = await context.Authors.AsNoTracking().ToListAsync();
                return View(model);
            }
        }

        [HttpGet] public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, LastName")] Author author)
        {
            await using var context = new LibraryDbContext();

            if (!ModelState.IsValid) { return View(author); }

            context.Add(author);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
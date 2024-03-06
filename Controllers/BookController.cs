using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class BookController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await using var db = new LibraryDbContext();
            return View(db.Authors.Include(author => author.Books).AsNoTracking());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await using var db = new LibraryDbContext();
            ViewBag.Authors = GenerateAuthorSelectList(db.Authors);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, AuthorID")] Book book)
        {
            await using var db = new LibraryDbContext();

            if (!ModelState.IsValid)
            {
                ViewBag.Auhtors = GenerateAuthorSelectList(db.Authors);
                return View(book);
            }

            db.Books.Add(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private static List<SelectListItem> GenerateAuthorSelectList(IEnumerable<Author> source) => source
            .Select(author => new SelectListItem($"{author.FirstName} {author.LastName}", author.AuthorID.ToString()))
            .ToList();
    }
}
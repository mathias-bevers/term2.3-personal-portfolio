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
            List<Author> authors = await db.Authors.Include(author => author.Books).AsNoTracking().ToListAsync();
            return View(authors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = HttpContext.Session.GetSessionObjectFromJson<User>(Settings.SESSION_USER_KEY);

            if (user is not { IsAdministrator: true })
            {
                HttpContext.Items["ErrorMessage"] = "This page can only be accessed by administrators.";
                return StatusCode(StatusCodes.Status401Unauthorized);
            }


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
                ViewBag.Authors = GenerateAuthorSelectList(db.Authors);
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
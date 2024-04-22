using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class BookController : Controller
    {
        // 10mb
        private const int MAX_BYTES = 10485760;

        public async Task<IActionResult> Index()
        {
            await using var db = new LibraryDbContext();
            List<Author> authors = await db.Authors.Include(author => author.Books).ThenInclude(book => book.Cover)
                .AsNoTracking().ToListAsync();
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
        public async Task<IActionResult> Create([Bind("Title, AuthorID, CoverFile")] Book book)
        {
            await using var db = new LibraryDbContext();

            if (!ModelState.IsValid)
            {
                ViewBag.Authors = GenerateAuthorSelectList(db.Authors);
                return View(book);
            }

            Book newbook = new()
            {
                Title = book.Title,
                AuthorID = book.AuthorID
            };

            if (book.CoverFile is { Length: > 0 })
            {
                using (var memoryStream = new MemoryStream())
                {
                    book.CoverFile.CopyTo(memoryStream);
                    if (memoryStream.Length < MAX_BYTES)
                    {
                        newbook.Cover = new BookCover
                        {
                            Bytes = memoryStream.ToArray(),
                            FileExtension = book.CoverFile.ContentType
                        };
                    }
                    else { ModelState.AddModelError("CoverFile", "The uploaded file is too large!"); }
                }
            }


            db.Books.Add(newbook);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private static List<SelectListItem> GenerateAuthorSelectList(IEnumerable<Author> source) => source
            .Select(author => new SelectListItem($"{author.FirstName} {author.LastName}", author.AuthorID.ToString()))
            .ToList();
    }
}
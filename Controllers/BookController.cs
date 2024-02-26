using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class BookController : Controller
    {
        private List<Author> authors;
        private List<SelectListItem> selectableAuthors;

        public BookController()
        {
            using var db = new LibraryDbContext();
            authors = db.Authors.Include(author => author.Books).AsNoTracking().ToList();

            selectableAuthors = authors.Select(author => new SelectListItem($"{author.FirstName} {author.LastName}",
                author.AuthorID.ToString())).ToList();
        }
        
        public IActionResult Index()
        {
                return View(authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Authors = selectableAuthors;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, AuthorID")] Book book)
        {
            await using var context = new LibraryDbContext();
            
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = selectableAuthors;
                return View(book);
            }
            
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
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
            using (var context = new LibraryDbContext())
            {
                List<Author> model = await context.Authors.Include(author => author.Books).AsNoTracking().ToListAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (var context = new LibraryDbContext())
            {
                List<SelectListItem> authors = await context.Authors.Select(author => new SelectListItem
                {
                    Value = author.AuthorId.ToString(),
                    Text = $"{author.FirstName} {author.LastName}"
                }).ToListAsync();
                ViewBag.Authors = authors;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, AuthorId")] Book book)
        {
            using (var context = new LibraryDbContext())
            {
                context.Books.Add(book);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}
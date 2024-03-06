using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessionLogin = HttpContext.Session.GetSessionObjectFromJson<User>(Settings.SESSION_USER_KEY);

            if (ReferenceEquals(sessionLogin, null)) { return RedirectToAction("Login"); }

            await using var db = new LibraryDbContext();
            List<User> users = await db.Users.ToListAsync();
            return View(users);
        }

        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid) { return View(user); }

            await using var db = new LibraryDbContext();

            User? userDetail = await db.Users.FirstOrDefaultAsync(
                x => x.UserName == user.UserName && x.Password == user.Password);

            if (ReferenceEquals(userDetail, null))
            {
                user.ErrorMessage = "Invalid credentials!";
                return View(user);
            }

            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, userDetail);
            return RedirectToAction("Index");
        }

        [HttpGet] public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user, string confirmPW)
        {
            await using var db = new LibraryDbContext();

            if (!ModelState.IsValid) { return View(user); }

            if (!confirmPW.Equals(user.Password))
            {
                ModelState.AddModelError("Password", "The passwords are not the same");
                return View(user);
            }

            if (await db.Users.AnyAsync(u => u.UserName.Equals(user.UserName)))
            {
                ModelState.AddModelError("UserName", $"The username \'{user.UserName}\' is already taken");
                return View(user);
            }

            user.IsAdministrator = false;

            db.Users.Add(user);
            await db.SaveChangesAsync();
            HttpContext.Session.SetSessionObjectAsJson(Settings.SESSION_USER_KEY, user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var currentAdmin = HttpContext.Session.GetSessionObjectFromJson<User>(Settings.SESSION_USER_KEY);
            if (ReferenceEquals(currentAdmin, null))
            {
                HttpContext.Items["ErrorMessage"] = "Only an administrator can edit users.";
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            using var db = new LibraryDbContext();
            User? user = db.Users.FirstOrDefault(u => u.UserID == id);
            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserID", "UserName, IsAdministrator")] User changedUser)
        {
            await using var db = new LibraryDbContext();

            var currentAdmin = HttpContext.Session.GetSessionObjectFromJson<User>(Settings.SESSION_USER_KEY);
            if (ReferenceEquals(currentAdmin, null))
            {
                HttpContext.Items["ErrorMessage"] = "Cannot edit users when not logged in as administrator.";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            if (changedUser.UserID == currentAdmin.UserID && !changedUser.IsAdministrator)
            {
                ModelState.AddModelError("IsAdministrator", "Cannot revoke own admin rights");
            }

            ModelState.Remove("Password");

            if (!ModelState.IsValid) { return View(changedUser); }

            User? user = await db.Users.FirstOrDefaultAsync(u => u.UserID == changedUser.UserID);

            if (ReferenceEquals(user, null))
            {
                HttpContext.Items["ErrorMessage"] = $"Could not find user with id: {changedUser.UserID}!";
                return StatusCode(StatusCodes.Status404NotFound);
            }

            user.UserName = changedUser.UserName;
            user.IsAdministrator = changedUser.IsAdministrator;

            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await using (var db = new LibraryDbContext())
                {
                    User? user = await db.Users.FirstOrDefaultAsync(u => u.UserID == id);

                    if (ReferenceEquals(user, null))
                    {
                        HttpContext.Items["ErrorMessage"] = $"Could not find user with id: {id}!";
                        return StatusCode(StatusCodes.Status404NotFound);
                    }

                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                HttpContext.Items["ErrorMessage"] = "Error deleting data";
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
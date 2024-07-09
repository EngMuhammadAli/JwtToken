using JwtToken.jwtDbContect;
using JwtToken.Models;
using JwtToken.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class UserRegistrationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtGenrate _jwtGenrate;
    public UserRegistrationController(ApplicationDbContext context, IOptions<PasswordSettings> passwordSettings, IJwtGenrate jwtGenrate)
    {
        _context = context;
        _jwtGenrate = jwtGenrate;
    }

    // GET: UserRegistration/Index
    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    // GET: UserRegistration/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: UserRegistration/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        if (user != null)
        {
            bool succes = _jwtGenrate.NewUser(user);
            if (succes == true)
            {
                TempData["SuccessMessage"] = "User Add Successflly.!";

                return RedirectToAction(nameof(Index));

            }
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: UserRegistration/Edit/5
    public IActionResult Edit(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _context.Users.FirstOrDefault(u => u.UserID == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: UserRegistration/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, User user)
    {
        if (id != user.UserID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        return View(user);
    }

    // GET: UserRegistration/Delete/5
    public IActionResult Delete(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _context.Users.FirstOrDefault(u => u.UserID == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: UserRegistration/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserID == id);
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // Method to validate password against settings
    public bool IsPasswordValid(string password)
    {
        var settings = _context.PasswordSettings.FirstOrDefault() ?? new PasswordSettings();

        // Implement your password validation logic based on password settings
        if (settings.RequireDigit && !password.Any(char.IsDigit))
            return false;

        if (settings.RequiredLength > 0 && password.Length < settings.RequiredLength)
            return false;

        if (settings.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
            return false;

        if (settings.RequireUppercase && !password.Any(char.IsUpper))
            return false;

        if (settings.RequireLowercase && !password.Any(char.IsLower))
            return false;

        return true;
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(u => u.UserID == id);
    }
}

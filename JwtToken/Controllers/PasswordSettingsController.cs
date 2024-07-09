using JwtToken.jwtDbContect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class PasswordSettingsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PasswordSettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /PasswordSettings
    public IActionResult Index()
    {
        var settings = _context.PasswordSettings.FirstOrDefault() ?? new PasswordSettings(); // Fetch settings or create new if none exists

        var viewModel = new PasswordSettingsViewModel
        {
            RequireDigit = settings.RequireDigit,
            RequiredLength = settings.RequiredLength,
            RequireNonAlphanumeric = settings.RequireNonAlphanumeric,
            RequireUppercase = settings.RequireUppercase,
            RequireLowercase = settings.RequireLowercase
        };

        return View(viewModel);
    }

    // POST: /PasswordSettings
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(PasswordSettingsViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var settings = _context.PasswordSettings.FirstOrDefault();
            if (settings == null)
            {
                // Create new settings if none exist
                settings = new PasswordSettings();
                _context.PasswordSettings.Add(settings);
            }

            // Update settings
            settings.RequireDigit = viewModel.RequireDigit;
            settings.RequiredLength = viewModel.RequiredLength;
            settings.RequireNonAlphanumeric = viewModel.RequireNonAlphanumeric;
            settings.RequireUppercase = viewModel.RequireUppercase;
            settings.RequireLowercase = viewModel.RequireLowercase;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // If model state is not valid, return the form with validation errors
        return View(viewModel);
    }
}

using JwtToken.jwtDbContect;
using JwtToken.Models;
using JwtToken.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JwtToken.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtGenrate _jwtGenrate;
        private readonly PasswordSettings _passwordSettings;

        public LoginController(ApplicationDbContext context, IOptions<PasswordSettings> passwordSettings, IJwtGenrate jwtGenrate)
        {
            _context = context;
            _jwtGenrate = jwtGenrate;
            _passwordSettings = passwordSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userPassword = _context.UserPassword.FirstOrDefault(s => s.Password == loginViewModel.Password);
                if (userPassword != null)
                {
                    var user = _context.Users.FirstOrDefault(s => s.UserID == userPassword.UserID);
                    if (user != null)
                    {
                        if (user.ConfirmPasswordReset)
                        {
                            return RedirectToAction("ResetPassword");
                        }
                        else
                        {
                            // Implement your successful login logic here
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            if (ModelState.IsValid)
            {
                var userPassword = _context.UserPassword.FirstOrDefault(s => s.Password == passwordResetViewModel.Password);
                if (userPassword != null && userPassword.Password == passwordResetViewModel.Password)
                {
                    if (IsPasswordValid(passwordResetViewModel.Password))
                    {
                        UpdateUser(userPassword.UserID);
                        userPassword.Password = passwordResetViewModel.ConfirmPassword;
                        _context.UserPassword.Update(userPassword);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View();
                    }
                   
                }
            }
            return View();
        }

        private bool UpdateUser(int userID)
        {
            var user = _context.Users.FirstOrDefault(s => s.UserID == userID);
            if (user != null)
            {
                user.ConfirmPasswordReset = false;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(s => s.Email == model.Email);
                if (user != null)
                {
                    bool update = _jwtGenrate.UpdateUser(user);
                    if (update)
                    {
                        return RedirectToAction("ResetPassword");
                    }
                    else {
                        TempData["SuccessMessage"] = "User Not Exsist.!";
                    }

                }
            }
            return View(model);
        }

        private bool IsPasswordValid(string password)
        {
            var settings = _passwordSettings;

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
    }
}

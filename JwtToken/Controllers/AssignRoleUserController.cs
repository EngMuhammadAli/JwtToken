using JwtToken.jwtDbContect;
using JwtToken.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtToken.Controllers
{
    public class AssignRoleUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignRoleUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }


        public IActionResult Assign(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = _context.Roles.ToList();
            var userRoleViewModel = new UserRoleViewModel
            {
                UserId = user.UserID,
                Username = user.Name,
                SelectedRoles = _context.UserRoles
                                         .Where(ur => ur.UserID == userId)
                                         .Select(ur => ur.Role.RoleName)
                                         .ToList()
            };

            // Ensure Roles is initialized with actual roles
            userRoleViewModel.Roles = roles;

            return View(userRoleViewModel);
        }


        [HttpPost]
        public IActionResult Assign(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process role assignment logic
                var userRoles = _context.UserRoles.Where(ur => ur.UserID == model.UserId);
                _context.UserRoles.RemoveRange(userRoles);

                foreach (var roleName in model.SelectedRoles)
                {
                    var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        _context.UserRoles.Add(new UserRole { UserID = model.UserId, RoleID = role.RoleID });
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return the view with the model to display validation errors
            return View(model);
        }

    }
}

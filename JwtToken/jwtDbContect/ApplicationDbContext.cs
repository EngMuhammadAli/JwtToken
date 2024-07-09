using JwtToken.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtToken.jwtDbContect
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PasswordSettings> PasswordSettings { get; set; }

        public DbSet<UserPassword> UserPassword { get; set; }


    }
}

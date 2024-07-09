using System.ComponentModel.DataAnnotations;

namespace JwtToken.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public bool ConfirmPasswordReset { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace JwtToken.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required]
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace JwtToken.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
}

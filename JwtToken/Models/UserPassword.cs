using System.ComponentModel.DataAnnotations;

namespace JwtToken.Models
{
    public class UserPassword
    {
        [Key]
        public int MyProperty { get; set; }
        public int  UserID { get; set; }
        public string Password { get; set; }
    }
}

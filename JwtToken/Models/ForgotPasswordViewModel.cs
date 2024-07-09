using System.ComponentModel.DataAnnotations;

namespace JwtToken.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

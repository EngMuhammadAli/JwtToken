// PasswordSettingsViewModel.cs
using System.ComponentModel.DataAnnotations;

public class PasswordSettingsViewModel
{
    [Display(Name = "Require Digit")]
    public bool RequireDigit { get; set; }

    [Display(Name = "Minimum Length")]
    [Range(1, int.MaxValue, ErrorMessage = "Minimum length must be at least 1")]
    public int RequiredLength { get; set; }

    [Display(Name = "Require Non-Alphanumeric")]
    public bool RequireNonAlphanumeric { get; set; }

    [Display(Name = "Require Uppercase")]
    public bool RequireUppercase { get; set; }

    [Display(Name = "Require Lowercase")]
    public bool RequireLowercase { get; set; }
}

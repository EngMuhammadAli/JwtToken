// PasswordSettings.cs
using System.ComponentModel.DataAnnotations;

public class PasswordSettings
{
    [Key]
    public int Id { get; set; }

    public bool RequireDigit { get; set; }

    public int RequiredLength { get; set; }

    public bool RequireNonAlphanumeric { get; set; }

    public bool RequireUppercase { get; set; }

    public bool RequireLowercase { get; set; }
}

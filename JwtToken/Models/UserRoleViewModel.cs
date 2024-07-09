namespace JwtToken.Models
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<Role> Roles { get; set; }
        public List<string> SelectedRoles { get; set; } // Add this property

    }
}

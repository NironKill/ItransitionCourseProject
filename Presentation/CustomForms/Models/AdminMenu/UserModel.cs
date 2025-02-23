namespace CustomForms.Models.AdminMenu
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}

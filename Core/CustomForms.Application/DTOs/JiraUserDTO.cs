namespace CustomForms.Application.DTOs
{
    public class JiraUserDTO
    {
        public string AccountId { get; set; } 
        public string Self { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; } 
        public string Key { get; set; } 
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
    }
}

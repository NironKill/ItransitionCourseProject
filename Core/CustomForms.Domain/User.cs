using Microsoft.AspNetCore.Identity;

namespace CustomForms.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool LockoutEnabled { get; set; } = false;
        public string SalesforceAccountId { get; set; } = string.Empty;

        public ICollection<Template> Templates { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Form> Forms { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}

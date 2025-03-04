using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AccountId { get; set; }
        public int TicketJiraId { get; set; }
        public string Key { get; set; }
        public string TicketUrl { get; set; }

        public User User { get; set; }
    }
}

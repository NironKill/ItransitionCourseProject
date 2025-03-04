namespace CustomForms.Application.DTOs
{
    public class TicketDTO
    {
        public string AccountId { get; set; }
        public string Summary { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string PageUrl { get; set; }
        public string TicketUrl { get; set; }
        public string Key { get; set; }
        public int TicketJiraId { get; set; }
        public Guid UserId { get; set; }
    }
}

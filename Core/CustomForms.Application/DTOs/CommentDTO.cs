namespace CustomForms.Application.DTOs
{
    public class CommentDTO
    {
        public Guid TemplateId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CommentedAt { get; set; }
    }
}

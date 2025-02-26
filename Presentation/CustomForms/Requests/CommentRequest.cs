namespace CustomForms.Requests
{
    public class CommentRequest
    {
        public Guid TemplateId { get; set; }
        public string Content { get; set; }
    }
}

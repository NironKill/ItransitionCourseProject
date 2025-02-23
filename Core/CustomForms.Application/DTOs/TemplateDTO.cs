namespace CustomForms.Application.DTOs
{
    public class TemplateDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public bool IsPublic { get; set; }

        public List<QuestionDTO> Questions { get; set; }
    }
}

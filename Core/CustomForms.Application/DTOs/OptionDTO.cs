namespace CustomForms.Application.DTOs
{
    public class OptionDTO
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}

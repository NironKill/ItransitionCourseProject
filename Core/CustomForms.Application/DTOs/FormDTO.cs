namespace CustomForms.Application.DTOs
{
    public class FormDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }
        public DateTime FilledAt { get; set; }

        public ICollection<AnswerDTO> Answers;
    }
}

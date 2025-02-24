namespace CustomForms.Application.DTOs
{
    public class FormDTO
    {
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }

        public ICollection<AnswerDTO> Answers;
    }
}

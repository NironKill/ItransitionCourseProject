using CustomForms.Application.DTOs;

namespace CustomForms.Requests
{
    public class FillRequest
    {
        public Guid TemplateId { get; set; }
        public Guid UserId { get; set; }

        public ICollection<AnswerDTO> Answers { get; set; }
    }
}

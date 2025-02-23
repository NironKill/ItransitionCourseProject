using CustomForms.Domain;

namespace CustomForms.Application.DTOs
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Type { get; set; }

        public List<OptionDTO> Options { get; set; }
    }
}

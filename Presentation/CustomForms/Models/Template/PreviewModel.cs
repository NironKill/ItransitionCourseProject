using CustomForms.Application.DTOs;

namespace CustomForms.Models.Template
{
    public class PreviewModel
    {
        public Guid TemplateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<QuestionDTO> Questions { get; set; }
    }
}

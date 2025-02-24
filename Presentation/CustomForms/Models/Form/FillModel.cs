using CustomForms.Application.DTOs;

namespace CustomForms.Models.Form
{
    public class FillModel
    {
        public Guid TemplateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<QuestionDTO> Questions { get; set; }
    }
}

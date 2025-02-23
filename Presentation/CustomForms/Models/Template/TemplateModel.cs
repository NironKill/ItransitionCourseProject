using CustomForms.Application.DTOs;

namespace CustomForms.Models.Template
{
    public class TemplateModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<TopicDTO> Topics { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}

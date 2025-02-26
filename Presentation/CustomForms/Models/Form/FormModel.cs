using CustomForms.Application.DTOs;

namespace CustomForms.Models.Form
{
    public class FormModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FilledAt { get; set; }

        public ICollection<AnswerDTO> Answers { get; set; }
        public ICollection<QuestionDTO> Questions { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public Guid QuestionId { get; set; }
        public string Response { get; set; }

        public Form Form { get; set; }
        public Question Question { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Type { get; set; } 

        public Template Template { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}

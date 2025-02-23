using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Form
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }
        public int FilledAt { get; set; }

        public User User { get; set; }
        public Template Template { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}

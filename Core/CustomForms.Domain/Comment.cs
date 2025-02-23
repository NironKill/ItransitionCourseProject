using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }
        public string Content { get; set; }
        public int CommentedAt { get; set; }

        public User User { get; set; }
        public Template Template { get; set; }
    }
}

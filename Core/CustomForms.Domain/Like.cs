using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }
        public int LikeAt { get; set; }

        public User User { get; set; }
        public Template Template { get; set; }
    }
}

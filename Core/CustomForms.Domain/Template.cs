using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Template
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public bool IsPublic { get; set; }

        public User User { get; set; }
        public Topic Topic { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Form> Forms { get; set; }
        public ICollection<TemplateTag> TemplateTags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Template> Templates { get; set; }
        public ICollection<TemplateTag> TemplateTags { get; set; }
    }
}

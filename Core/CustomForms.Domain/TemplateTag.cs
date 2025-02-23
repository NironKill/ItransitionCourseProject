using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class TemplateTag
    {
        [Key]
        public int Id { get; set; }
        public Guid TemplateId { get; set; }
        public Guid TagId { get; set; }

        public Template Template { get; set; }
        public Tag Tag { get; set; }
    }
}

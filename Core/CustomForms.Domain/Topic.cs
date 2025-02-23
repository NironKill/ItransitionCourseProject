using System.ComponentModel.DataAnnotations;

namespace CustomForms.Domain
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Template> Templates { get; set; }
    }
}

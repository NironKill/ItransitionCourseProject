namespace CustomForms.Models.Home
{
    public class TemplateListModel
    {
        public Guid Id { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public bool IsPublic { get; set; }
    }
}

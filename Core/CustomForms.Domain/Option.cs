namespace CustomForms.Domain
{
    public class Option
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public Question Question { get; set; }
    }
}

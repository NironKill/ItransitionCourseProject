namespace CustomForms.Infrastructure.Responses.JIra
{
    public class IssuelistResponse
    {
        public List<Issue> Issues { get; init; }
    }
    public class Issue
    {
        public string Id { get; init; }
        public string Key { get; init; }
        public Fields Fields { get; init; }
    }
    public class Fields
    {
        public Priority Priority { get; init; }
        public Status Status { get; init; }
        public string Summary { get; init; }
    }
    public class Priority
    {
        public string Name { get; init; }
    }
    public class Status
    {
        public string Name { get; init; }
    }
}

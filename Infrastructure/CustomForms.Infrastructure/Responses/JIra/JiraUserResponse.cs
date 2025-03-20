namespace CustomForms.Infrastructure.Responses.JIra
{
    public class JiraUserResponse
    {
        public string AccountId { get; init; }
        public string Self { get; init; }
        public string Name { get; init; }
        public string AccountType { get; init; }
        public string Key { get; init; }
        public string EmailAddress { get; init; }
        public string DisplayName { get; init; }
        public bool Active { get; init; }
    }
}

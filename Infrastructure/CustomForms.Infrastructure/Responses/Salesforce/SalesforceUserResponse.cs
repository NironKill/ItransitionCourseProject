namespace CustomForms.Infrastructure.Responses.Salesforce
{
    public class SalesforceUserResponse
    {
        public Account Account { get; init; }
        public Contact Contact { get; init; }
    }
    public class Account
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
    public class Contact
    {
        public string Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string AccountId { get; init; }
        public DateTime Birthdate { get; init; }
    }
}

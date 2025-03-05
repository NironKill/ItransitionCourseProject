namespace CustomForms.Persistence.Responses.Salesforce
{
    public class AccessTokenResponse
    {
        public string access_token { get; init; }
        public string instance_url { get; init; }
        public string id { get; init; }
        public string token_type { get; init; }
        public string issued_at { get; init; }
        public string signature { get; init; }
    }
}

using CustomForms.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CustomForms.Application.Services.Implementations
{
    public class ApiService : IApiService
    {
        private readonly IConfiguration _configuration;

        public ApiService(IConfiguration configuration) => _configuration = configuration;

        public string GetJiraApiToken()
        {
            string token = _configuration["Jira:ApiToken"];

            if (!string.IsNullOrEmpty(token))
                return token;

            string envString = Environment.GetEnvironmentVariable("JIRA_API_TOKEN");

            return envString;
        }
        public string GetJiraURL()
        {
            string url = _configuration["Jira:Url"];

            if (!string.IsNullOrEmpty(url))
                return url;

            string envString = Environment.GetEnvironmentVariable("JIRA_URL");

            return envString;
        }
        public string GetJiraUsername()
        {
            string username = _configuration["Jira:Username"];

            if (!string.IsNullOrEmpty(username))
                return username;

            string envString = Environment.GetEnvironmentVariable("JIRA_USERNAME");

            return envString;
        }
    }
}

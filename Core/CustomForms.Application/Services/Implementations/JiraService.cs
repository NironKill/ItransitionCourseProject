using CustomForms.Application.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace CustomForms.Application.Services.Implementations
{
    public class JiraService : IJiraService
    {
        private readonly IApiService _api;

        public JiraService(IApiService api) => _api = api;
        
        public AuthenticationHeaderValue GetAuthenticationToken() => new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_api.GetJiraUsername()}:{_api.GetJiraApiToken()}")));

        public string GetUrl() => _api.GetJiraURL();
    }
}

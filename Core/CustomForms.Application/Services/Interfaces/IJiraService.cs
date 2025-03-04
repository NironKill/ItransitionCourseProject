using System.Net.Http.Headers;

namespace CustomForms.Application.Services.Interfaces
{
    public interface IJiraService
    {
        AuthenticationHeaderValue GetAuthenticationToken();
        string GetUrl();
    }
}

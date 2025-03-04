namespace CustomForms.Application.Services.Interfaces
{
    public interface IApiService
    {
        string GetJiraUsername();
        string GetJiraApiToken();
        string GetJiraURL();
    }
}

namespace CustomForms.Application.Services.Interfaces
{
    public interface ISalesforceService
    {
        Dictionary<string, string> GetFormData();
        string GetUrl();
    }
}

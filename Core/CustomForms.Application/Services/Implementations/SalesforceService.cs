using CustomForms.Application.Common.Configurations;
using CustomForms.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CustomForms.Application.Services.Implementations
{
    public class SalesforceService : ISalesforceService
    {
        private readonly IApiService _api;

        public SalesforceService(IApiService api) => _api = api;

        public Dictionary<string, string> GetFormData() => new Dictionary<string, string>()
        {
            { "grant_type", "password" },
            { "client_id", _api.GetApiConfiguration(SalesforceOption.Key) },
            { "client_secret", _api.GetApiConfiguration(SalesforceOption.Secret) },
            { "username", _api.GetApiConfiguration(SalesforceOption.Username) },
            { "password", _api.GetApiConfiguration(SalesforceOption.Password) }
        };
        public string GetUrl() => _api.GetApiConfiguration(SalesforceOption.Url);
    }
}

using CustomForms.Application.Common.Configurations;
using CustomForms.Application.Services.Interfaces;

namespace CustomForms.Application.Services.Implementations
{
    public class SalesforceService : ISalesforceService
    {
        private readonly IApiService _api;

        public SalesforceService(IApiService api) => _api = api;

        public string GetApiToken() => _api.GetApiConfiguration(SalesforceOption.Token);
        public string GetUrl() => _api.GetApiConfiguration(SalesforceOption.Url);
    }
}

using CustomForms.Infrastructure.DTOs;
using CustomForms.Infrastructure.Responses.Salesforce;

namespace CustomForms.Infrastructure.Services.Salesforce
{
    public interface ISalesforceApiService
    {
        Task<SalesforceUserResponse> GetUser(string accountId);
        Task<string> GetAsseccToken();

        Task<string> CreateAccount(SalesforceUserDTO dto);
    }
}

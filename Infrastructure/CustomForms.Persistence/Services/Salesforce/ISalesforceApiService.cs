using CustomForms.Persistence.DTOs;
using CustomForms.Persistence.Responses.Salesforce;

namespace CustomForms.Persistence.Services.Salesforce
{
    public interface ISalesforceApiService
    {
        Task<SalesforceUserResponse> GetUser(string accountId);
        Task<string> GetAsseccToken();

        Task<string> CreateAccount(SalesforceUserDTO dto);
    }
}

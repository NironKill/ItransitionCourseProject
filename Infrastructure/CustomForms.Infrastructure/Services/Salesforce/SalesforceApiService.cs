using CustomForms.Application.Services.Interfaces;
using CustomForms.Infrastructure.DTOs;
using CustomForms.Infrastructure.Responses.Salesforce;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CustomForms.Infrastructure.Services.Salesforce
{
    public class SalesforceApiService : ISalesforceApiService
    {
        private readonly ISalesforceService _salesforce;

        public SalesforceApiService(ISalesforceService salesforce) => _salesforce = salesforce;

        public async Task<string> GetAsseccToken()
        {
            Dictionary<string, string> formData = _salesforce.GetFormData();

            using (HttpClient client = new HttpClient())
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(formData);
                HttpResponseMessage response = await client.PostAsync($"{_salesforce.GetUrl()}/services/oauth2/token", content);

                response.EnsureSuccessStatusCode();
                try
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AccessTokenResponse>(jsonResponse);
                    var resultToken = result is AccessTokenResponse responseData ? responseData.access_token : null;

                    return resultToken;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
        public async Task<string> CreateAccount(SalesforceUserDTO dto)
        {
            string token = await GetAsseccToken();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var account = new
                {
                    Name = $"{dto.FirstName} {dto.LastName}"
                };

                var jsonAccount = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
                HttpResponseMessage responseAccount = await client.PostAsync($"{_salesforce.GetUrl()}/services/data/v57.0/sobjects/account/", jsonAccount);

                if (responseAccount.IsSuccessStatusCode)
                {
                    string jsonResponse = await responseAccount.Content.ReadAsStringAsync();
                    Account result = JsonConvert.DeserializeObject<Account>(jsonResponse);

                    var contact = new
                    {
                        Title = dto.Description,
                        dto.FirstName,
                        dto.LastName,
                        dto.Email,
                        dto.Phone,
                        AccountId = result.Id,
                        dto.Birthdate
                    };

                    var jsonContact = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseContact = await client.PostAsync($"{_salesforce.GetUrl()}/services/data/v57.0/sobjects/contact/", jsonContact);

                    return result.Id;
                }
                return string.Empty;
            }
        }

        public async Task<SalesforceUserResponse> GetUser(string accountId)
        {
            string token = await GetAsseccToken();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage accountResponse = await client.GetAsync($"{_salesforce.GetUrl()}/services/data/v57.0/sobjects/account/{accountId}");

                string accountJson = await accountResponse.Content.ReadAsStringAsync();
                Account account = JsonConvert.DeserializeObject<Account>(accountJson);

                string query = $"SELECT Id, FirstName, LastName, Email, Phone, AccountId FROM Contact WHERE AccountId='{accountId}'";
                HttpResponseMessage contactResponse = await client.GetAsync($"{_salesforce.GetUrl()}/services/data/v57.0/query?q={Uri.EscapeDataString(query)}");

                if (!contactResponse.IsSuccessStatusCode)
                    return null;

                string contactJson = await contactResponse.Content.ReadAsStringAsync();
                Contact contact = JsonConvert.DeserializeObject<Contact>(contactJson);

                SalesforceUserResponse userResponse = new SalesforceUserResponse()
                {
                    Account = account,
                    Contact = contact
                };
                return userResponse;
            }
        }
    }
}

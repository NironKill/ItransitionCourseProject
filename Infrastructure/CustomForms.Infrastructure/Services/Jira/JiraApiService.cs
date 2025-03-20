using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Infrastructure.Responses.JIra;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace CustomForms.Infrastructure.Services.Jira
{
    public class JiraApiService : IJiraApiService
    {
        private readonly IJiraService _jira;
        private readonly ITicketRepository _ticket;

        public JiraApiService(IJiraService jira, ITicketRepository ticket)
        {
            _jira = jira;
            _ticket = ticket;
        }

        public async Task<bool> CreateIssue(JiraUserResponse jiraUserResponse, TicketDTO ticketDTO, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

                var issue = new
                {
                    fields = new
                    {
                        project = new { key = "MFLP" },
                        summary = ticketDTO.Summary,
                        reporter = new { id = jiraUserResponse.AccountId ?? "" },
                        priority = new { name = ticketDTO.Priority },
                        issuetype = new { name = "Bug" },
                        customfield_10090 = ticketDTO.PageUrl,
                        description = new
                        {
                            version = 1,
                            type = "doc",
                            content = new[] {
                                 new {
                                     type = "paragraph",
                                     content = new []{
                                         new {
                                             type = "text",
                                             text =  ticketDTO.Summary
                                         }
                                     }
                                 }
                            }
                        },
                    }
                };

                var response = await client.PostAsJsonAsync($"{_jira.GetUrl()}/rest/api/3/issue", issue);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    IssueResponse issueDto = JsonConvert.DeserializeObject<IssueResponse>(responseData);

                    if (issueDto == null)
                        return false;

                    ticketDTO.TicketUrl = $"{_jira.GetUrl()}/browse/{issueDto.Key}";
                    ticketDTO.AccountId = jiraUserResponse.AccountId;
                    ticketDTO.Key = issueDto.Key;
                    ticketDTO.TicketJiraId = issueDto.Id;

                    await _ticket.Create(ticketDTO, cancellationToken);

                    return true;
                }
                return false;
            }
        }

        public async Task<JiraUserResponse> CreateUser(UserDTO dto)
        {
            var payload = new
            {
                emailAddress = dto.Email,
                displayName = dto.Name,
                products = new[] { "jira-software" }
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_jira.GetUrl()}/rest/api/3/user", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<JiraUserResponse>(responseData);
                }
                return null;
            }
        }

        public async Task<JiraUserResponse> GetUserByEmail(string email)
        {
            var destination = $"{_jira.GetUrl()}/rest/api/3/user/search?query={email}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

                var response = await client.GetAsync(destination);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsJson = await response.Content.ReadAsStringAsync();
                    List<JiraUserResponse> users = JsonConvert.DeserializeObject<List<JiraUserResponse>>(responseAsJson);

                    return users?.FirstOrDefault();
                }
                return null;
            }
        }

        public async Task<List<TicketDTO>> GetAllTicketByAccountId(string accountId)
        {
            var destination = $"{_jira.GetUrl()}/rest/api/3/search?jql=assignee={accountId} OR reporter={accountId}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

                var response = await client.GetAsync(destination);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsJson = await response.Content.ReadAsStringAsync();
                    IssuelistResponse issues = JsonConvert.DeserializeObject<IssuelistResponse>(responseAsJson);

                    List<TicketDTO> tickets = new List<TicketDTO>();

                    foreach (var issue in issues.Issues)
                    {
                        TicketDTO ticket = new TicketDTO()
                        {
                            TicketUrl = $"{_jira.GetUrl()}/browse/{issue.Key}",
                            Status = issue.Fields.Status.Name,
                            Priority = issue.Fields.Priority.Name,
                            Summary = issue.Fields.Summary
                        };
                        tickets.Add(ticket);
                    }
                    return tickets;
                }
                return null;
            }
        }
    }
}

using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace CustomForms.Persistence.Services.Jira
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

        public async Task<bool> CreateIssue(JiraUserDTO jiraUserDTO, TicketDTO ticketDTO, CancellationToken cancellationToken)
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
                        reporter = new { id = jiraUserDTO.AccountId ?? "" },
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
                    IssueDTO issueDto = JsonConvert.DeserializeObject<IssueDTO>(responseData);

                    if (issueDto == null)
                        return false;

                    ticketDTO.TicketUrl = $"{_jira.GetUrl()}/browse/{issueDto.Key}";
                    ticketDTO.AccountId = jiraUserDTO.AccountId;
                    ticketDTO.Key = issueDto.Key;
                    ticketDTO.TicketJiraId = issueDto.Id;

                    await _ticket.Create(ticketDTO, cancellationToken); 

                    return true;
                }
            }
            return false;
        }

        public async Task<JiraUserDTO> CreateUser(UserDTO dto)
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
                    return JsonConvert.DeserializeObject<JiraUserDTO>(responseData);
                }
            }
            return null;
        }

        public async Task<JiraUserDTO> GetUserByEmail(string email)
        {
            var destination = $"{_jira.GetUrl()}/rest/api/3/user/search?query={email}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

                var response = await client.GetAsync(destination);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsJson = await response.Content.ReadAsStringAsync();
                    List<JiraUserDTO> users = JsonConvert.DeserializeObject<List<JiraUserDTO>>(responseAsJson);

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
                    IssuelistDTO issues = JsonConvert.DeserializeObject<IssuelistDTO>(responseAsJson);

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

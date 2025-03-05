using CustomForms.Application.DTOs;
using CustomForms.Persistence.Responses.JIra;

namespace CustomForms.Persistence.Services.Jira
{
    public interface IJiraApiService
    {
        Task<JiraUserResponse> CreateUser(UserDTO dto);
        Task<bool> CreateIssue(JiraUserResponse jiraUserResponse, TicketDTO ticketDTO, CancellationToken cancellationToken);

        Task<JiraUserResponse> GetUserByEmail(string email);
        Task<List<TicketDTO>> GetAllTicketByAccountId(string accountId);
    }
}

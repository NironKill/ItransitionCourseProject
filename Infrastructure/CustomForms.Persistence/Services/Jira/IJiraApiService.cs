using CustomForms.Application.DTOs;

namespace CustomForms.Persistence.Services.Jira
{
    public interface IJiraApiService
    {
        Task<JiraUserDTO> CreateUser(UserDTO dto);
        Task<bool> CreateIssue(JiraUserDTO jiraUserDTO, TicketDTO ticketDTO, CancellationToken cancellationToken);

        Task<JiraUserDTO> GetUserByEmail(string email);
        Task<List<TicketDTO>> GetAllTicketByAccountId(string accountId);
    }
}

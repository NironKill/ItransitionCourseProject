using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Infrastructure.Responses.JIra;
using CustomForms.Infrastructure.Services.Jira;
using CustomForms.Models.Jira;
using CustomForms.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class JiraController : Controller
    {
        private readonly IAccessTokenService _accessToken;
        
        private readonly IUserRepository _user;
        private readonly IJiraApiService _jiraApi;
        private readonly ITicketRepository _ticket;

        public JiraController(IAccessTokenService accessToken, IUserRepository user, IJiraApiService jiraApi, ITicketRepository ticket)
        {
            _accessToken = accessToken;
            _user = user;
            _jiraApi = jiraApi;
            _ticket = ticket;
        }

        [HttpGet("Table")]
        [Authorize]
        public async Task<IActionResult> Table()
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            string accountId = await _ticket.GetAccountId(user.Id) ?? string.Empty;

            if (string.IsNullOrEmpty(accountId))
                return RedirectToAction("Index", "Home");

            List<TicketDTO> ticketsDTO = await _jiraApi.GetAllTicketByAccountId(accountId);

            List<TicketModel> models = new List<TicketModel>();
            foreach (TicketDTO ticketDTO in ticketsDTO)
            {
                TicketModel model = new TicketModel()
                {
                    Link = ticketDTO.TicketUrl,
                    Summary = ticketDTO.Summary,
                    Priority = ticketDTO.Priority,
                    Status = ticketDTO.Status
                };
                models.Add(model);
            }
            return View(models);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateTicket([FromBody]TicketRequest request, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            JiraUserResponse jiraUserResponse = await _jiraApi.GetUserByEmail(user.Email) ?? await _jiraApi.CreateUser(user);

            if (jiraUserResponse is null)
                return BadRequest("User creation failed.");

            TicketDTO ticketDTO = new TicketDTO()
            {
                Summary = request.Summary,
                Priority = request.Priority,
                PageUrl = request.PageUrl,
                UserId = user.Id
            };

            bool issueEx = await _jiraApi.CreateIssue(jiraUserResponse, ticketDTO, cancellationToken);

            if (issueEx is false)
                return BadRequest("Issue has not been created.");

            return Ok();
        }
    }
}

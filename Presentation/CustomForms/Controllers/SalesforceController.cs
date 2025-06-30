using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Infrastructure.DTOs;
using CustomForms.Infrastructure.Services.Salesforce;
using CustomForms.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class SalesforceController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly ISalesforceApiService _salesforce;
        private readonly IUserRepository _user;

        public SalesforceController(IAccessTokenService accessToken, ISalesforceApiService salesforce, IUserRepository user)
        {
            _accessToken = accessToken;
            _salesforce = salesforce;
            _user = user;
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]SalesforceUserRequest request)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.Get(x => x.Email == userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrEmpty(user.SalesforceAccountId))
                return BadRequest("The salesforce contact is already been created.");

            SalesforceUserDTO salesforceUserDTO = new SalesforceUserDTO()
            {
                Birthdate = request.Birthdate,
                Description = request.Description,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = user.Email,
                Phone = request.Phone,
            };

            string accountId = await _salesforce.CreateAccount(salesforceUserDTO);
            if (string.IsNullOrEmpty(accountId))
                return BadRequest("Connection to Salesforce was failed. Please try again.");

            await _user.Update(x => x.Id == user.Id, (entity) => 
            {
                entity.SalesforceAccountId = accountId;
            });

            return Ok();
        }
    }
}

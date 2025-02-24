using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using CustomForms.Models.Form;
using CustomForms.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class FormController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly ITemplateRepository _template;
        private readonly IUserRepository _user;
        private readonly IFormRepository _form;

        public FormController(IAccessTokenService accessToken, ITemplateRepository template, IUserRepository user, IFormRepository form)
        {
            _accessToken = accessToken;
            _template = template;
            _user = user;
            _form = form;
        }

        [HttpGet("Fill/{id}")]
        public async Task<IActionResult> Fill(Guid id)
        {
            Guid userId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                UserDTO user = await _user.GetByEmail(userEmail);
                userId = user.Id;
            }

            TemplateDTO templateDto = await _template.GetById(id);

            FillModel model = new FillModel()
            {
                TemplateId = id,
                Description = templateDto.Description,
                Title = templateDto.Title,
                Questions = templateDto.Questions,
            };

            ViewBag.CurrentUserId = userId;
            return View(model);
        }

        [HttpPost("Fill")]
        [Authorize]
        public async Task<IActionResult> Fill([FromBody]FillRequest request, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            FormDTO dto = new FormDTO()
            {
                TemplateId = request.TemplateId,
                UserId = request.UserId,

                Answers = request.Answers,
            };

            await _form.Create(dto, cancellationToken);

            return Ok();
        }

        [HttpGet("View/{id}")]
        public IActionResult Views()
        {
            return View();
        }
    }
}

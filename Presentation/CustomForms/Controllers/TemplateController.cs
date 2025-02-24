using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using CustomForms.Models.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class TemplateController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly IUserRepository _user;
        private readonly ITopicRepository _topic;
        private readonly ITemplateRepository _template;

        public TemplateController(IAccessTokenService accessToken, IUserRepository userRepository, ITopicRepository topic, ITemplateRepository template)
        {
            _accessToken = accessToken;

            _user = userRepository;
            _topic = topic;
            _template = template;
        }

        [HttpGet("Create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ICollection<TopicDTO> dtos = await _topic.GetAll(); 

            TemplateModel model = new TemplateModel()
            {
                Topics = dtos
            };

            return View(model);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create(TemplateModel model, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO userDTO = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (userDTO.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            List<QuestionDTO> questionList = JsonConvert.DeserializeObject<List<QuestionDTO>>($"{ModelState["Questions"].RawValue}");

            TemplateDTO dto = new TemplateDTO()
            {
                UserId = userDTO.Id,
                Description = model.Description,
                Title = model.Title,
                TopicId = model.TopicId,
                Tags = model.Tag,
                IsPublic = model.IsPublic,
                Questions = questionList
            };

            Guid id = await _template.Create(dto, cancellationToken);

            return Redirect($"Edit/{id}");
        }

        [HttpGet("Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO userDTO = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (userDTO.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            TemplateDTO template = await _template.GetById(id);

            if (userDTO.Id == template.UserId || userDTO.Role == Role.Admin.ToString())
            {
                ICollection<TopicDTO> topics = await _topic.GetAll();

                TemplateModel model = new TemplateModel()
                {
                    Id = template.Id,
                    UserId = template.UserId,
                    TopicId = template.TopicId,
                    Description = template.Description,
                    IsPublic = template.IsPublic,
                    Title = template.Title,
                    Tag = template.Tags,
                    Topics = topics,
                    Questions = template.Questions
                };
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(TemplateModel model, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO userDTO = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (userDTO.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            List<QuestionDTO> questionList = JsonConvert.DeserializeObject<List<QuestionDTO>>($"{ModelState["Questions"].RawValue}");

            TemplateDTO template = await _template.GetById(model.Id);

            if (userDTO.Id == template.UserId || userDTO.Role == Role.Admin.ToString())
            {
                TemplateDTO dto = new TemplateDTO()
                {
                    Id = model.Id,
                    UserId = userDTO.Id,
                    TopicId = model.TopicId,
                    Description = model.Description,
                    IsPublic = model.IsPublic,
                    Title = model.Title,
                    Tags = model.Tag,
                    Questions = questionList
                };

                await _template.Update(dto, cancellationToken);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Preview/{id}")]
        [Authorize]
        public IActionResult Preview()
        {
            return View();
        }
    }
}

using CustomForms.Application.Common.Constants;
using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using CustomForms.Models;
using CustomForms.Models.Home;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly IUserRepository _user;
        private readonly ITemplateRepository _template;

        public HomeController(IAccessTokenService accessToken, IUserRepository user, ITemplateRepository template)
        {
            _accessToken=accessToken;

            _user = user;
            _template = template;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<TemplateDTO> templatedtos = await _template.GetAll();
            ICollection<UserDTO> userDTOs = await _user.GetAll();

            List<TemplateListModel> models = new List<TemplateListModel>();
            foreach (TemplateDTO dto in templatedtos)
            {
                string userName = userDTOs.Where(x => x.Id == dto.UserId).Select(x => x.Name).FirstOrDefault();
                Topics topic = (Topics)dto.TopicId;
                TemplateListModel model = new TemplateListModel()
                {
                    Id = dto.Id,
                    IsPublic = dto.IsPublic,
                    Tags = dto.Tags,
                    Title = dto.Title,
                    Topic = topic.ToString(),
                    Author = userName
                };
                models.Add(model);
            }
            return View(models);
        }

        [HttpPost]
        public IActionResult CultureManagement(string? culture, string? returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture ?? "en")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(returnUrl ?? "/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

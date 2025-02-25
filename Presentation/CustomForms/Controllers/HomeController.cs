using CustomForms.Application.Common.Constants;
using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using CustomForms.Models;
using CustomForms.Models.Home;
using Microsoft.AspNetCore.Components.Web;
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
        private readonly ILikeRepository _like;

        public HomeController(IAccessTokenService accessToken, IUserRepository user, ITemplateRepository template, ILikeRepository like)
        {
            _accessToken=accessToken;

            _user = user;
            _template = template;
            _like = like;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<TemplateDTO> templateDtos = await _template.GetAll();
            ICollection<UserDTO> userDtos = await _user.GetAll();
            ICollection<LikeDTO> likeDtos= await _like.GetAll();

            Guid userId = Guid.Empty;
            string role = "User";
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                UserDTO user = await _user.GetByEmail(userEmail);
                userId = user.Id;
                role = user.Role;
            }

            List<TemplateListModel> models = new List<TemplateListModel>();
            bool isLiked = false;
            foreach (TemplateDTO dto in templateDtos)
            {
                int numberLikes = likeDtos.Where(x => x.TemplateId == dto.Id).Count();

                if (likeDtos.Where(x => x.UserId == userId && x.TemplateId == dto.Id).Any())
                    isLiked = true;

                string userName = userDtos.Where(x => x.Id == dto.UserId).Select(x => x.Name).FirstOrDefault();
                Topics topic = (Topics)dto.TopicId;
                TemplateListModel model = new TemplateListModel()
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    IsPublic = dto.IsPublic,
                    Tags = dto.Tags,
                    Title = dto.Title,
                    Topic = topic.ToString(),
                    Author = userName,
                    IsLiked = isLiked,
                    NumberLikes = numberLikes
                };
                models.Add(model);
            }

            ViewBag.Role = role;
            ViewBag.CurrentUserId = userId;
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

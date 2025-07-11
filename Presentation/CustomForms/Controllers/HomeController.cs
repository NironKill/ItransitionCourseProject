using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Models;
using CustomForms.Models.Home;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomForms.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly IUserRepository _user;
        private readonly ITemplateRepository _template;
        private readonly ILikeRepository _like;
        private readonly ICommentRepository _comment;
        private readonly IFormRepository _form;

        public HomeController(IAccessTokenService accessToken, IUserRepository user, ITemplateRepository template, ILikeRepository like, ICommentRepository comment, IFormRepository form)
        {
            _accessToken=accessToken;

            _user = user;
            _template = template;
            _like = like;
            _comment = comment;
            _form = form;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<TemplateDTO> templateDtos = await _template.GetAll();
            ICollection<UserDTO> userDtos = await _user.GetAll();
            ICollection<LikeDTO> likeDtos= await _like.GetAll();
            ICollection<CommentDTO> commentDtos = await _comment.GetAll();
            ICollection<FormDTO> formDtos = new List<FormDTO>();

            Guid userId = Guid.Empty;
            string role = "User";
            bool isSalesforceId = false;
            string firstname = string.Empty;
            string lastname = string.Empty;
            string fullname = string.Empty;
            string email = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                UserDTO user = await _user.Get(x => x.Email == userEmail);
                userId = user.Id;
                role = user.Role;
                formDtos = await _form.GetAllByUserId(userId);
                string[] nameSplit = user.Name.Split(new[] { ' ' }, 2);
                firstname = nameSplit[0];
                lastname = string.Join(" ", nameSplit.Skip(1));
                fullname = user.Name;
                email = user.Email;

                if (!string.IsNullOrEmpty(user.SalesforceAccountId))
                    isSalesforceId = true;
            }

            List<TemplateListModel> templateListModels = new List<TemplateListModel>();
            bool isLiked = false;
            foreach (TemplateDTO dto in templateDtos)
            {
                ICollection<CommentDTO> comments = commentDtos.Where(x => x.TemplateId == dto.Id).OrderByDescending(x => x.CommentedAt).ToList();
                FormDTO form = formDtos.Where(x => x.TemplateId == dto.Id).OrderByDescending(x => x.FilledAt).FirstOrDefault();

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
                    Description = dto.Description,
                    Tags = dto.Tags,
                    Title = dto.Title,
                    Topic = topic.ToString(),
                    Author = userName,
                    IsLiked = isLiked,
                    NumberLikes = numberLikes,

                    Comments = comments,
                    Form = form
                };
                templateListModels.Add(model);
            }

            ViewBag.Fullname = fullname;
            ViewBag.Email = email;
            ViewBag.Firstname = firstname;
            ViewBag.Lastname = lastname;
            ViewBag.IsSalesforceId = isSalesforceId;
            ViewBag.Role = role;
            ViewBag.CurrentUserId = userId;
            return View(templateListModels);
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

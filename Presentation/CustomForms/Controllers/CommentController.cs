using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly IUserRepository _user;
        private readonly ICommentRepository _comment;

        public CommentController(IAccessTokenService accessToken, IUserRepository user, ICommentRepository comment)
        {
            _accessToken = accessToken;
            _user = user;
            _comment = comment;
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]CommentRequest request, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            CommentDTO dto = new CommentDTO()
            {
                UserId = user.Id,
                Content = request.Content,
                TemplateId = request.TemplateId,
            };

            await _comment.Create(dto, cancellationToken);

            return Ok();
        }
    }
}

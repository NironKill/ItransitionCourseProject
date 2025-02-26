using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class LikeController : Controller
    {
        private readonly IAccessTokenService _accessToken;

        private readonly IUserRepository _user;
        private readonly ILikeRepository _like;

        public LikeController(IAccessTokenService accessToken, IUserRepository user, ILikeRepository like)
        {
            _accessToken=accessToken;

            _user = user;
            _like = like;
        }

        [HttpPost("Add/{id}")]
        [Authorize]
        public async Task<IActionResult> Add(Guid id, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            LikeDTO dto = new LikeDTO()
            {
                UserId = user.Id,
                TemplateId = id
            };

            await _like.Add(dto, cancellationToken);

            return Ok();
        }

        [HttpDelete("Remove/{id}")]
        [Authorize]
        public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO user = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (user.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            LikeDTO dto = new LikeDTO()
            {
                UserId = user.Id,
                TemplateId = id
            };

            await _like.Remove(dto, cancellationToken);

            return Ok();
        }
    }
}

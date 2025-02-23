using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using CustomForms.Models.AdminMenu;
using CustomForms.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class AdminMenuController : Controller
    {
        private readonly IUserRepository _user;
        private readonly IAccessTokenService _accessToken;
        public AdminMenuController(IUserRepository user, IAccessTokenService accessToken)
        {
            _user = user;
            _accessToken = accessToken;
        }

        [HttpGet("Manage")]
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO userDTO = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (userDTO.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (userDTO.Role == Role.Admin.ToString())
            {
                ICollection<UserDTO> dtos = await _user.GetAll(userDTO.Id);

                List<UserModel> models = new List<UserModel>();

                foreach (UserDTO dto in dtos)
                {
                    UserModel model = new UserModel()
                    {
                        Email = dto.Email,
                        Role = dto.Role,
                        Name = dto.Name,
                        LockoutEnabled = dto.LockoutEnabled
                    };
                    models.Add(model);
                }
                return View(models);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] UserRequest request)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO dto = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (dto.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (request.Emails.Count == default)
                return BadRequest();

            if (dto.Role == Role.Admin.ToString())
            {
                await _user.Remove(request.Emails);

                return Ok();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Lock")]
        [Authorize]
        public async Task<IActionResult> Lock([FromBody] UserRequest request)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO dto = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (dto.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (request.Emails.Count == default)
                return BadRequest();

            if (dto.Role == Role.Admin.ToString())
            {
                await _user.Lock(request.Emails);

                return Ok();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Unlock")]
        [Authorize]
        public async Task<IActionResult> Unlock([FromBody] UserRequest request)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO dto = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (dto.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (request.Emails.Count == default)
                return BadRequest();

            if (dto.Role == Role.Admin.ToString())
            {
                await _user.Unlock(request.Emails);

                return Ok();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Privilege")]
        [Authorize]
        public async Task<IActionResult> Privilege([FromBody] UserRequest request)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO dto = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (dto.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (request.Emails.Count == default)
                return BadRequest();

            if (dto.Role == Role.Admin.ToString())
            {
                await _user.Privilege(request.Emails);

                return Ok();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Deprivilege")]
        [Authorize]
        public async Task<IActionResult> Deprivilege([FromBody] UserRequest request)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO dto = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (dto.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            if (request.Emails.Count == default)
                return BadRequest();

            if (dto.Role == Role.Admin.ToString())
            {
                await _user.Deprivilege(request.Emails);

                return Ok();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("DeprivilegeYourself")]
        [Authorize]
        public async Task<IActionResult> DeprivilegeYourself()
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            bool isInvalid = await _accessToken.ValidateToken(userIdClaim.Subject.Name);
            if (!isInvalid)
                return RedirectToAction("Login", "Account");

            UserDTO dto = await _user.GetByEmail(userIdClaim.Subject.Name);
            if (dto.LockoutEnabled)
                return RedirectToAction("Login", "Account");

            ICollection<string> emails = new List<string>
            {
                dto.Email
            };

            await _user.Deprivilege(emails);

            return Ok();
        }
    }
}

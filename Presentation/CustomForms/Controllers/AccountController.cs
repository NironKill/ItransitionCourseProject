using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _account;
        private readonly IUserRepository _user;

        public AccountController(IAccountService account, IUserRepository user)
        {
            _account = account;
            _user = user;
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model, CancellationToken cancellationToken)
        {
            RegisterDTO dto = new RegisterDTO()
            {
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (ModelState.IsValid)
            {
                bool isRegistered = await _account.Registration(dto, cancellationToken);
                if (isRegistered)
                    return RedirectToAction("Index", "Home");
                                
                ModelState.AddModelError(string.Empty, "An error occurred while registering the user.");
            }

            return View(model);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            IList<AuthenticationScheme> externalLogins = await _account.GetExternalLogins();

            LoginModel model = new LoginModel()
            {
                ReturnUrl = $"{Request.Scheme}://{Request.Host}",
                ExternalLogins = externalLogins
            };
            return View(model);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
        {
            LoginDTO dto = new LoginDTO()
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            if (ModelState.IsValid)
            {
                bool isLoggedIn = await _account.Login(dto);
                if (isLoggedIn)              
                    return RedirectToAction("Index", "Home");
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            model.ReturnUrl = $"{Request.Scheme}://{Request.Host}/";
            model.ExternalLogins = await _account.GetExternalLogins();
            return View(model);
        }

        [HttpGet("ExternalAddLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            string redirectUrl = Url.Action(
                action: "ExternalLoginCallback",
                controller: "Account",
                values: new { ReturnUrl = returnUrl }
            );

            AuthenticationProperties properties = _account.GetProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [Route("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError, CancellationToken cancellationToken)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError is not null)
                return Content($"<script>alert('Error from external provider: {remoteError}'); window.close();</script>", "text/html");

            ExternalLoginInfo info = await _account.GetAccountInfo();
            if (info is null)
                return Content($"<script>alert('Error loading external login information.'); window.close();</script>", "text/html");

            var signInResult = await _account.ExternalSignIn(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (signInResult.Succeeded)
            {
                UserCreateDTO userDTO = new UserCreateDTO()
                {
                    Email = email,
                    UserName = email,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                };
                await _user.UpdateName(userDTO);

                return Content($@"<script>localStorage.setItem('authSuccess', 'true'); window.close();</script>", "text/html");
            }                       
            
            if (email is not null)
            {
                bool userExistence = await _user.UserExistenceCheckByMail(email);
                if (!userExistence)
                {
                    UserCreateDTO userDTO = new UserCreateDTO()
                    {
                        Email = email,
                        UserName = email,
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                    };
                    await _user.Create(userDTO, cancellationToken);
                }
                await _account.ExternalAddLogin(email, info);
              
                return Content($@"<script>localStorage.setItem('authSuccess', 'true'); window.close();</script>", "text/html");
            }

            return Content($"<script>alert('Email claim not received. Please contact support.'); window.close();</script>", "text/html");
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _account.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}

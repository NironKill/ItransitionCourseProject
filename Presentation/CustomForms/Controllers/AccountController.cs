using CustomForms.Application.DTOs;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Models;
using CustomForms.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace CustomForms.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _account;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IAccountService account, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _account = account;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
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
                {
                    HttpResponse response = _httpContextAccessor.HttpContext.Response;

                    response.Cookies.Append("Email", model.Email, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    response.Cookies.Append("Name", $"{model.FirstName} {model.LastName}", new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "An error occurred while registering the user.");
            }

            return View(model);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            LoginDTO dto = new LoginDTO()
            {
                Email = model.Email,
                Password = model.Password
            };

            if (ModelState.IsValid)
            {
                bool isLoggedIn = await _account.Login(dto);
                if (isLoggedIn)
                {
                    UserDTO user = await _userRepository.GetByEmail(dto.Email);

                    HttpResponse response = _httpContextAccessor.HttpContext.Response;

                    response.Cookies.Append("Email", model.Email, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    response.Cookies.Append("Name", $"{user.Name}", new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _account.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}

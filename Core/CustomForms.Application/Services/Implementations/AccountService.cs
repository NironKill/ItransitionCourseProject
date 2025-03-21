﻿using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CustomForms.Application.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IAccessTokenService _accessToken;

        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, IAccessTokenService accessToken)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _accessToken = accessToken;
        }

        public async Task<bool> Registration(RegisterDTO dto, CancellationToken cancellationToken)
        {
            User user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                LockoutEnabled = false
            };

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Role.Admin.ToString()))
                {
                    IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(Role.Admin.ToString()));
                    if (!roleResult.Succeeded)
                        return false;
                }
                if (!await _roleManager.RoleExistsAsync(Role.User.ToString()))
                {
                    IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(Role.User.ToString()));
                    if (!roleResult.Succeeded)
                        return false;
                }

                await _accessToken.Create(user.Id, cancellationToken);

                IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, Role.Admin.ToString());
                if (!addToRoleResult.Succeeded)
                    return false;

                await _signInManager.SignInAsync(user, false);
                return true;
            }
            return false;
        }
        public async Task<bool> Login(LoginDTO dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                dto.RememberMe,
                false);

            return result.Succeeded;
        }
        public async Task ExternalAddLogin(string email, ExternalLoginInfo info)
        {
            User user = await _userManager.FindByEmailAsync(email);

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        public async Task Logout() => await _signInManager.SignOutAsync();
        public async Task<SignInResult> ExternalSignIn(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor) =>
            await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);   
        public async Task<IList<AuthenticationScheme>> GetExternalLogins() => (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        public async Task<ExternalLoginInfo> GetAccountInfo() => await _signInManager.GetExternalLoginInfoAsync();
        public AuthenticationProperties GetProperties(string provider, string redirectUrl) => _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    }
}

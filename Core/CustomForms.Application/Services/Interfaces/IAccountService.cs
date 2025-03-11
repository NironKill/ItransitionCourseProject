using CustomForms.Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CustomForms.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Registration(RegisterDTO dto, CancellationToken cancellationToken);
        Task<bool> Login(LoginDTO dto);
        Task ExternalAddLogin(string email, ExternalLoginInfo info);
        Task<SignInResult> ExternalSignIn(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task Logout();

        Task<IList<AuthenticationScheme>> GetExternalLogins();
        Task<ExternalLoginInfo> GetAccountInfo();
        AuthenticationProperties GetProperties(string provider, string redirectUrl);
    }
}

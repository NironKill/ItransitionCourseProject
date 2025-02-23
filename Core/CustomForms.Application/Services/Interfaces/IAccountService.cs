using CustomForms.Application.DTOs;

namespace CustomForms.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Registration(RegisterDTO dto, CancellationToken cancellationToken);
        Task<bool> Login(LoginDTO dto);
        Task Logout();
    }
}

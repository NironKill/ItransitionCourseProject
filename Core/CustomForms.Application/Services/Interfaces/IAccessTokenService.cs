namespace CustomForms.Application.Services.Interfaces
{
    public interface IAccessTokenService
    {
        Task<string> Create(Guid userId, CancellationToken cancellationToken);
        Task<bool> ValidateToken(string token);
        Task InvalidateToken(Guid userId, CancellationToken cancellationToken);
    }
}

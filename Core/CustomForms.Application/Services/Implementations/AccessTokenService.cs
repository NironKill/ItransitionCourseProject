using CustomForms.Application.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Services.Implementations
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IApplicationDbContext _context;

        public AccessTokenService(IApplicationDbContext context) => _context = context;

        public async Task<string> Create(Guid userId, CancellationToken cancellationToken)
        {
            string token = Guid.NewGuid().ToString();

            bool isExist = await _context.UserTokens.AnyAsync(t => t.UserId == userId && t.LoginProvider == "API");

            if (!isExist)
                _context.UserTokens.Add(new IdentityUserToken<Guid>
                {
                    UserId = userId,
                    LoginProvider = "API",
                    Name = "APIToken",
                    Value = token
                });

            await _context.SaveChangesAsync(cancellationToken);
            return token;
        }
        public async Task<bool> ValidateToken(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
                return false;

            IdentityUserToken<Guid> tokenEntry = await _context.UserTokens.FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == "API");
            if (tokenEntry is null)
                return false;

            return true;
        }
        public async Task InvalidateToken(Guid userId, CancellationToken cancellationToken)
        {
            IdentityUserToken<Guid> tokenEntry = await _context.UserTokens.FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == "API");

            if (tokenEntry is not null)
            {
                _context.UserTokens.Remove(tokenEntry);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

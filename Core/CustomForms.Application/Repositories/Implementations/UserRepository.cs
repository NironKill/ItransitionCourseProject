using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(IApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ICollection<UserDTO>> GetAll(Guid id)
        {
            List<User> users = await _context.Users.Where(x => x.Id != id).ToListAsync();

            List<UserDTO> dtos = new List<UserDTO>();
            foreach (User user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                UserDTO dto = new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}",
                    Role = roles.FirstOrDefault(),
                    LockoutEnabled = user.LockoutEnabled
                };
                dtos.Add(dto);
            }
            return dtos;
        }
        public async Task<ICollection<UserDTO>> GetAll()
        {
            List<User> users = await _context.Users.ToListAsync();

            List<UserDTO> dtos = new List<UserDTO>();
            foreach (User user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                UserDTO dto = new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}",
                    Role = roles.FirstOrDefault(),
                    LockoutEnabled = user.LockoutEnabled
                };
                dtos.Add(dto);
            }
            return dtos;
        }
        public async Task<UserDTO> GetByEmail(string email)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == email);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            UserDTO dto = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName}",
                LockoutEnabled = user.LockoutEnabled,
                Role = roles.FirstOrDefault(),
                SalesforceAccountId = user.SalesforceAccountId
            };

            return dto;
        }
        public async Task<UserDTO> GetById(Guid userId)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == userId);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            UserDTO dto = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName}",
                LockoutEnabled = user.LockoutEnabled,
                Role = roles.FirstOrDefault()
            };

            return dto;
        }
        public async Task<UserDTO> GetByApiToken(string apiToken)
        {
            Guid userId = await _context.UserTokens.Where(t => t.Value == apiToken && t.LoginProvider == "API").Select(x => x.UserId).FirstOrDefaultAsync();

            User user = _context.Users.FirstOrDefault(x => x.Id == userId);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            UserDTO dto = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName}",
                LockoutEnabled = user.LockoutEnabled,
                Role = roles.FirstOrDefault()
            };

            return dto;
        }
        public async Task Lock(ICollection<string> listEmail)
        {
            foreach (string email in listEmail)
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                user.LockoutEnabled = true;
                user.LockoutEnd = new DateTimeOffset(DateTime.UtcNow.AddYears(100));

                await _userManager.UpdateAsync(user);
            }
        }
        public async Task Unlock(ICollection<string> listEmail)
        {
            foreach (string email in listEmail)
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                user.LockoutEnabled = false;
                user.LockoutEnd = new DateTimeOffset(DateTime.UtcNow);

                await _userManager.UpdateAsync(user);
            }
        }
        public async Task Remove(ICollection<string> listEmail)
        {
            foreach (string email in listEmail)
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                await _userManager.DeleteAsync(user);
            }
        }
        public async Task Update(UserDTO dto)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);

            user.SalesforceAccountId = dto.SalesforceAccountId;

            await _userManager.UpdateAsync(user);
        }
        public async Task Privilege(ICollection<string> emails)
        {
            foreach (string email in emails)
            {
                User user = await _userManager.FindByEmailAsync(email);

                IList<string> currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                await _userManager.AddToRoleAsync(user, Role.Admin.ToString());
            }
        }
        public async Task Deprivilege(ICollection<string> emails)
        {
            foreach (string email in emails)
            {
                User user = await _userManager.FindByEmailAsync(email);
                
                IList<string> currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                await _userManager.AddToRoleAsync(user, Role.User.ToString());
            }
        }
    }
}

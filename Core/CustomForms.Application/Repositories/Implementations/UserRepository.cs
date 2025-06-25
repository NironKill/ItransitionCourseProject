using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CustomForms.Application.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IAccessTokenService _accessToken;

        public UserRepository(IApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IAccessTokenService accessToken)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _accessToken = accessToken;
        }

        public async Task<ICollection<UserDTO>> GetAll(Guid? id = null)
        {
            IQueryable<User> query = _context.Users;

            if (id.HasValue)
                query = query.Where(x => x.Id != id);

            List<User> users = await query.ToListAsync();

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
        public async Task<UserDTO> Get(Expression<Func<User, bool>> predicate)
        {
            User user = await _context.Users.FirstOrDefaultAsync(predicate);

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
        public async Task Create(UserCreateDTO dto, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                LockoutEnabled = false
            };
            await _userManager.CreateAsync(user);
            if (!await _roleManager.RoleExistsAsync(Role.Admin.ToString()))            
                await _roleManager.CreateAsync(new IdentityRole<Guid>(Role.Admin.ToString()));

            
            if (!await _roleManager.RoleExistsAsync(Role.User.ToString()))           
                await _roleManager.CreateAsync(new IdentityRole<Guid>(Role.User.ToString()));
            
            await _accessToken.Create(user.Id, cancellationToken);

            await _userManager.AddToRoleAsync(user, Role.Admin.ToString());
        }
        public async Task UpdateAccountId(UserDTO dto)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);

            user.SalesforceAccountId = dto.SalesforceAccountId;

            await _userManager.UpdateAsync(user);
        }
        public async Task UpdateName(UserCreateDTO dto)
        {
            User user = await _userManager.FindByEmailAsync(dto.Email);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

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
        public async Task<bool> UserExistenceCheckByMail(string email) => await _context.Users.AnyAsync(x => x.Email == email);
    }
}

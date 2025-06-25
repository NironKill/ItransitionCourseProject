using CustomForms.Application.DTOs;
using CustomForms.Domain;
using System.Linq.Expressions;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Unlock(ICollection<string> emails);
        Task Lock(ICollection<string> emails);
        Task Remove(ICollection<string> emails);
        Task Privilege(ICollection<string> emails);
        Task Deprivilege(ICollection<string> emails);
        Task UpdateName(UserCreateDTO dto);
        Task UpdateAccountId(UserDTO dto);
        Task Create(UserCreateDTO dto, CancellationToken cancellationToken);
        Task<bool> UserExistenceCheckByMail(string email);

        Task<ICollection<UserDTO>> GetAll(Guid? id = null);
        Task<UserDTO> Get(Expression<Func<User, bool>> predicate);
    }
}

using CustomForms.Application.DTOs;

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

        Task<ICollection<UserDTO>> GetAll(Guid id);
        Task<ICollection<UserDTO>> GetAll();
        Task<UserDTO> GetById(Guid userId);
        Task<UserDTO> GetByEmail(string email);
        Task<UserDTO> GetByApiToken(string apiToken);
    }
}

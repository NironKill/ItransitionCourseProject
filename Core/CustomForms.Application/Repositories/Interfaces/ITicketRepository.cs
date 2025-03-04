using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task Create(TicketDTO dto, CancellationToken cancellationToken);

        Task<ICollection<TicketDTO>> GetAllByUserEmail(string email);
        Task<string> GetAccountId(Guid userId);
    }
}

using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface IFormRepository
    {
        Task Create(FormDTO dto, CancellationToken cancellationToken);
        Task<ICollection<FormDTO>> GetAllByUserId(Guid userId);
        Task<FormDTO> GetById(Guid formId);

    }
}

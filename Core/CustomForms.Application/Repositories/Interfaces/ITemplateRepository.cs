using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ITemplateRepository
    {
        Task<Guid> Create(TemplateDTO dto, CancellationToken cancellationToken);
        Task Update(TemplateDTO dto, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);

        Task<TemplateDTO> GetById(Guid templateId);
        Task<ICollection<TemplateDTO>> GetAll();
    }
}

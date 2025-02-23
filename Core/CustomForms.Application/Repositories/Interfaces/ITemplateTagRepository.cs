using CustomForms.Application.DTOs;
using CustomForms.Domain;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ITemplateTagRepository
    {
        Task Create(Guid templateId, List<string> tagNames, CancellationToken cancellationToken);
        Task Remove(Guid templateId, List<string> tagNames, CancellationToken cancellationToken);

        Task<ICollection<TemplateTagDTO>> GetAllById(Guid templateId);
    }
}

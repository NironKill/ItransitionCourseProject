using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task Create(IEnumerable<string> newTags, CancellationToken cancellationToken);

        Task<ICollection<TagDTO>> GetAll();
        Task<ICollection<TagDTO>> GetAllByTemplateId(Guid templateId);
    }
}

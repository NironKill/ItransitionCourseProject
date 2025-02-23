using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface IOptionRepository
    {
        Task Create(List<OptionDTO> dtos, Guid questionId, CancellationToken cancellationToken);
        Task Update(List<OptionDTO> dtos, CancellationToken cancellationToken);
        Task Delete(List<Guid> optionsId, CancellationToken cancellationToken);
        Task DeleteAll(Guid questionId, CancellationToken cancellationToken);
    }
}

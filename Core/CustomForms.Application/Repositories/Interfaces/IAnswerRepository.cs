using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task Create(ICollection<AnswerDTO> dtos, Guid formId, CancellationToken cancellationToken);
    }
}

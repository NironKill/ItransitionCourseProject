using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task Create(List<QuestionDTO> questions, Guid templateId, CancellationToken cancellationToken);
        Task Update(List<QuestionDTO> questions, CancellationToken cancellationToken);
        Task Delete(List<Guid> questionsId, CancellationToken cancellationToken);

        Task<ICollection<QuestionDTO>> GetAllByTemplateId(Guid templateId);
    }
}

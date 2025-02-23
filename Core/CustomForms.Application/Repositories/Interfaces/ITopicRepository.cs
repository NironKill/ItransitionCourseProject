using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ITopicRepository
    {
        Task Create();
        Task<ICollection<TopicDTO>> GetAll();
    }
}

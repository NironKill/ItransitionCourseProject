using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        Task Add(LikeDTO dto, CancellationToken cancellationToken);
        Task Remove(LikeDTO dto, CancellationToken cancellationToken);

        Task<ICollection<LikeDTO>> GetAll();
    }
}

using CustomForms.Application.DTOs;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task Create(CommentDTO dto, CancellationToken cancellationToken);
        Task<ICollection<CommentDTO>> GetAll();
    }
}

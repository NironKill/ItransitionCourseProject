using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _user;

        public CommentRepository(IApplicationDbContext context, IUserRepository user)
        {
            _context = context;
            _user = user;
        }

        public async Task Create(CommentDTO dto, CancellationToken cancellationToken)
        {
            Comment comment = new Comment()
            {
                Id = Guid.NewGuid(),
                TemplateId = dto.TemplateId,
                UserId = dto.UserId,
                Content = dto.Content,
                CommentedAt = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds)
            };
            
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<ICollection<CommentDTO>> GetAll()
        {
            List<Comment> comments = await _context.Comments.ToListAsync();

            ICollection<CommentDTO> dtos = new List<CommentDTO>();
            foreach (Comment comment in comments)
            {
                UserDTO user = await _user.GetById(comment.UserId);

                CommentDTO dto = new CommentDTO()
                {
                    TemplateId = comment.TemplateId,
                    Content = comment.Content,
                    CommentedAt = DateTime.UnixEpoch.AddSeconds(comment.CommentedAt).ToLocalTime(),
                    UserName = user.Name
                };
                dtos.Add(dto);
            }
            return dtos.OrderByDescending(x => x.CommentedAt).ToList();
        }
    }
}

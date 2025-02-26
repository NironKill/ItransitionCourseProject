using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class LikeRepository : ILikeRepository
    {
        private readonly IApplicationDbContext _context;

        public LikeRepository(IApplicationDbContext context) => _context = context;

        public async Task Add(LikeDTO dto, CancellationToken cancellationToken)
        {
            bool isLiked = await _context.Likes.Where(x => x.UserId == dto.UserId && x.TemplateId == dto.TemplateId).AnyAsync();
            if (isLiked)
                return;

            Like newLike = new Like()
            {
                Id = Guid.NewGuid(),
                TemplateId = dto.TemplateId,
                UserId = dto.UserId,
                LikeAt = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds)
            };

            await _context.Likes.AddAsync(newLike);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task Remove(LikeDTO dto, CancellationToken cancellationToken)
        {
            Like like = await _context.Likes.Where(x => x.UserId == dto.UserId && x.TemplateId == dto.TemplateId).FirstOrDefaultAsync();
            if (like is null)
                return;

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<ICollection<LikeDTO>> GetAll()
        {
            List<Like> likes = await _context.Likes.ToListAsync();

            ICollection<LikeDTO> dtos = new List<LikeDTO>();
            foreach (Like like in likes)
            {
                LikeDTO dto = new LikeDTO()
                {
                    UserId = like.UserId,
                    TemplateId = like.TemplateId
                };
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}

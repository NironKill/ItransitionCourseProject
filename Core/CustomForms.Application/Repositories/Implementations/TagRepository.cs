using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class TagRepository : ITagRepository
    {
        private readonly IApplicationDbContext _context;

        public TagRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(IEnumerable<string> newTags, CancellationToken cancellationToken)
        {
            List<Tag> tags = new List<Tag>();
            foreach (string nameTag in newTags)
            {
                Tag tag = new Tag()
                {
                    Id = Guid.NewGuid(),
                    Name = nameTag
                };
                tags.Add(tag);
            }
            await _context.Tags.AddRangeAsync(tags, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<TagDTO>> GetAll()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();

            return ModelFormation(tags);
        }

        public async Task<ICollection<TagDTO>> GetAllByTemplateId(Guid templateId)
        {
            List<Tag> tags = await _context.TemplateTags.Where(x => x.TemplateId == templateId).Select(x => x.Tag).ToListAsync();

            return ModelFormation(tags);
        }

        private List<TagDTO> ModelFormation(List<Tag> tags)
        {
            List<TagDTO> dtos = new List<TagDTO>();
            foreach (Tag tag in tags)
            {
                TagDTO dto = new TagDTO()
                {
                    Id = tag.Id,
                    Name = tag.Name
                };
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}

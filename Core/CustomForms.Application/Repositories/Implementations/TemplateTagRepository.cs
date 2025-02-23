using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class TemplateTagRepository : ITemplateTagRepository
    {
        private readonly IApplicationDbContext _context;

        public TemplateTagRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Guid templateId, List<string> tagNames, CancellationToken cancellationToken)
        {
            List<TemplateTag> templateTags = new List<TemplateTag>();
            foreach (string name in tagNames)
            {
                Guid tagId = _context.Tags.Where(x => x.Name == name).Select(x => x.Id).FirstOrDefault();

                TemplateTag newTemplateTag = new TemplateTag
                {
                    TemplateId = templateId,
                    TagId = tagId
                };

                templateTags.Add(newTemplateTag);
            }
            await _context.TemplateTags.AddRangeAsync(templateTags, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task Remove(Guid templateId, List<string> tagNames, CancellationToken cancellationToken)
        {
            List<TemplateTag> templateTags = new List<TemplateTag>();
            foreach (string name in tagNames)
            {
                TemplateTag templateTag = await _context.TemplateTags.Where(x => x.TemplateId == templateId && x.Tag.Name == name).FirstOrDefaultAsync();

                templateTags.Add(templateTag);
            }
            _context.TemplateTags.RemoveRange(templateTags);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<ICollection<TemplateTagDTO>> GetAllById(Guid templateId)
        {
            List<TemplateTag> templateTags = await _context.TemplateTags.Where(x => x.TemplateId == templateId).ToListAsync();

            List<TemplateTagDTO> dtos = new List<TemplateTagDTO>();
            foreach (TemplateTag templateTag in templateTags)
            {
                TemplateTagDTO dto = new TemplateTagDTO()
                {
                    Id = templateTag.Id,
                    TemplateId = templateTag.TemplateId,
                    TagId = templateTag.TagId
                };
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}

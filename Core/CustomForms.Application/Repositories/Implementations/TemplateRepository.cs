using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly ITagRepository _tag;
        private readonly ITemplateTagRepository _templateTag;
        private readonly IQuestionRepository _question;

        public TemplateRepository(IApplicationDbContext context, ITagRepository tag, ITemplateTagRepository templateTag, IQuestionRepository question)
        {
            _context = context;
            _tag = tag;
            _templateTag = templateTag;
            _question = question;
        }

        public async Task<Guid> Create(TemplateDTO dto, CancellationToken cancellationToken)
        {
            Template newTemplate = new Template()
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                TopicId = dto.TopicId,
                ImageURL = string.Empty,
                IsPublic = dto.IsPublic
            };
            await _context.Templates.AddAsync(newTemplate, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            List<string> tagNamesDTO = new List<string>();
            foreach (string name in dto.Tags.Split(" "))
                tagNamesDTO.Add(name);

            await CreateNewTag(tagNamesDTO, cancellationToken);

            await _templateTag.Create(newTemplate.Id, tagNamesDTO, cancellationToken);

            await _question.Create(dto.Questions, newTemplate.Id, cancellationToken);

            return newTemplate.Id;
        }
        public async Task Update(TemplateDTO dto, CancellationToken cancellationToken)
        {
            Template template = await _context.Templates.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            template.Title = dto.Title;
            template.Description = dto.Description;
            template.IsPublic = dto.IsPublic;
            template.TopicId = dto.TopicId;

            _context.Templates.Update(template);
            await _context.SaveChangesAsync(cancellationToken);

            List<string> tagNamesDTO = new List<string>();
            foreach (string name in dto.Tags.Split(" "))
                tagNamesDTO.Add(name);

            await CreateNewTag(tagNamesDTO, cancellationToken);

            ICollection<TagDTO> tags = await _tag.GetAllByTemplateId(template.Id);
            List<string> tagNamesDB = tags.Select(x => x.Name).ToList();

            List<string> deleteTags = tagNamesDB.Except(tagNamesDTO).ToList();
            if (deleteTags.Any())
                await _templateTag.Remove(template.Id, deleteTags, cancellationToken);

            List<string> addTags = tagNamesDTO.Except(tagNamesDB).ToList();
            if (addTags.Any())
                await _templateTag.Create(template.Id, addTags, cancellationToken);

            List<Guid> questionsIdDb = await _context.Questions.Where(x => x.TemplateId == template.Id).Select(x => x.Id).ToListAsync();
            List<Guid> questionsIdDTO = dto.Questions.Select(x => x.Id).ToList();
            List<Guid> deleteQuestions = questionsIdDb.Except(questionsIdDTO).ToList();
            if (deleteQuestions.Any())
                await _question.Delete(deleteQuestions, cancellationToken);

            List<QuestionDTO> questionCreate = dto.Questions.Where(x => x.Id == Guid.Empty).ToList();
            if (questionCreate.Any())
                await _question.Create(questionCreate, template.Id, cancellationToken);

            List<QuestionDTO> questionUpdate = dto.Questions.Where(x => x.Id != Guid.Empty).ToList();
            if (questionUpdate.Any())
                await _question.Update(questionUpdate, cancellationToken);
        }
        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            Template template = await _context.Templates.FirstOrDefaultAsync(x => x.Id == id);

            _context.Templates.Remove(template);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<TemplateDTO> GetById(Guid templateId)
        {
            TemplateDTO dto = await _context.Templates.Where(t => t.Id == templateId).Select(t => new TemplateDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                Title = t.Title,
                Description = t.Description,
                TopicId = t.TopicId,
                Tags = string.Join(" ", t.TemplateTags.Select(tt => tt.Tag.Name)),
                IsPublic = t.IsPublic,
                Questions = t.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Description = q.Description,
                    Type = q.Type,
                    Order = q.Order,
                    Options = q.Options.Select(o => new OptionDTO
                    {
                        Id = o.Id,
                        QuestionId = o.QuestionId,
                        Description = o.Description,
                        Order = o.Order
                    }).OrderBy(o => o.Order).ToList()
                }).OrderBy(q => q.Order).ToList()
            })
            .FirstOrDefaultAsync();

            return dto;
        }

        private async Task CreateNewTag(List<string> tagNamesDTO, CancellationToken cancellationToken)
        {
            ICollection<TagDTO> tags = await _tag.GetAll();
            List<string> tagNamesDB = tags.Select(x => x.Name).ToList();
            IEnumerable<string> newTags = tagNamesDTO.Except(tagNamesDB);
            if (newTags.Any())
                await _tag.Create(newTags, cancellationToken);
        }

        public async Task<ICollection<TemplateDTO>> GetAll()
        {
            List<Template> templates = await _context.Templates.ToListAsync();

            List<TemplateDTO> dtos = new List<TemplateDTO>();
            foreach (Template template in templates)
            {
                ICollection<TagDTO> tags = await _tag.GetAllByTemplateId(template.Id);
                TemplateDTO dto = new TemplateDTO()
                {
                    Id = template.Id,
                    UserId = template.UserId,
                    Title = template.Title,
                    Description = template.Description,
                    TopicId = template.TopicId,
                    Tags = string.Join(" ", tags.Select(x => x.Name)),
                    IsPublic = template.IsPublic
                };
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}

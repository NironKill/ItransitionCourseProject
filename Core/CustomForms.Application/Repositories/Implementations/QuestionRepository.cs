using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CustomForms.Application.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IOptionRepository _option;

        public QuestionRepository(IApplicationDbContext context, IOptionRepository option)
        {
            _context = context;
            _option = option;
        }

        public async Task Create(List<QuestionDTO> questions, Guid templateId, CancellationToken cancellationToken)
        {
            foreach (QuestionDTO dto in questions)
            {
                Question newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    TemplateId = templateId,
                    Description = dto.Description,
                    Type = dto.Type,
                    Order = dto.Order
                };
                await _context.Questions.AddAsync(newQuestion, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                if (dto.Options.Any())
                    await _option.Create(dto.Options, newQuestion.Id, cancellationToken);
            }
        }
        public async Task Update(List<QuestionDTO> questions, CancellationToken cancellationToken)
        {
            foreach (QuestionDTO dto in questions)
            {
                Question question = await _context.Questions.Where(x => x.Id == dto.Id).FirstOrDefaultAsync(); 

                if (question.Type == (int)QuestionType.Checkbox)
                {
                    if (dto.Type != (int)QuestionType.Checkbox)
                        await _option.DeleteAll(question.Id, cancellationToken);
                }

                question.Order = dto.Order;
                question.Description = dto.Description;
                question.Type = dto.Type;

                _context.Questions.Update(question);
                await _context.SaveChangesAsync(cancellationToken);

                List<Guid> optionsIdDb = await _context.Options.Where(x => x.QuestionId == question.Id).Select(x => x.Id).ToListAsync();
                List<Guid> optionsIdDTO = dto.Options.Select(x => x.Id).ToList();
                List<Guid> deleteOptions = optionsIdDb.Except(optionsIdDTO).ToList();
                if (deleteOptions.Any())
                    await _option.Delete(deleteOptions, cancellationToken);

                List<OptionDTO> optionCreate = dto.Options.Where(x => x.Id == Guid.Empty).ToList();
                if (optionCreate.Any())
                    await _option.Create(optionCreate, question.Id, cancellationToken);

                List<OptionDTO> optionUpdate = dto.Options.Where(x => x.Id != Guid.Empty).ToList();
                if (optionUpdate.Any())
                    await _option.Update(optionUpdate, cancellationToken);
            }

        }
        public async Task Delete(List<Guid> questionsId, CancellationToken cancellationToken)
        {
            List<Question> questions = new List<Question>();
            foreach (Guid questionId in questionsId)
            {
                Question question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

                questions.Add(question);
            }
            _context.Questions.RemoveRange(questions);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<QuestionDTO>> GetAllByTemplateId(Guid templateId)
        {
            List<QuestionDTO> dtos = await _context.Questions.Where(x => x.TemplateId == templateId).Select(q => new QuestionDTO
            {
                Id= q.Id,
                Description= q.Description,
                Order= q.Order,
                Type = q.Type,
                TemplateId = q.TemplateId,
                Options = q.Options.Select(o => new OptionDTO
                {
                    Id = o.Id,
                    Description = o.Description,
                    Order = o.Order,
                    QuestionId = o.QuestionId
                }).ToList()
            }).ToListAsync();

            return dtos;
        }
    }
}

using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CustomForms.Application.Repositories.Implementations
{
    public class OptionRepository : IOptionRepository
    {
        private readonly IApplicationDbContext _context;

        public OptionRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(List<OptionDTO> dtos, Guid questionId, CancellationToken cancellationToken)
        {
            List<Option> options = new List<Option>();
            foreach (OptionDTO dto in dtos)
            {
                Option newOption = new Option
                {
                    Id = Guid.NewGuid(),
                    QuestionId = questionId,
                    Description = dto.Description,
                    Order = dto.Order
                };
                options.Add(newOption);
            }
            await _context.Options.AddRangeAsync(options, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task Update(List<OptionDTO> dtos, CancellationToken cancellationToken)
        {
            List<Option> options = new List<Option>();
            foreach (OptionDTO dto in dtos)
            {
                Option option = await _context.Options.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

                option.Description = dto.Description;
                option.Order = dto.Order;

                options.Add(option);
            }
            _context.Options.UpdateRange(options);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task Delete(List<Guid> optionsId, CancellationToken cancellationToken)
        {
            List<Option> options = new List<Option>();

            foreach (Guid optionId in optionsId)
            {
                Option option = await _context.Options.Where(x => x.Id == optionId).FirstOrDefaultAsync();

                options.Add(option);
            }
            _context.Options.RemoveRange(options);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAll(Guid questionId, CancellationToken cancellationToken)
        {
            List<Option> options = await _context.Options.Where(x => x.QuestionId == questionId).ToListAsync();

            _context.Options.RemoveRange(options);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

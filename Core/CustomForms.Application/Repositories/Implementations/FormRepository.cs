using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;

namespace CustomForms.Application.Repositories.Implementations
{
    public class FormRepository : IFormRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IAnswerRepository _answer;

        public FormRepository(IApplicationDbContext context, IAnswerRepository answer)
        {
            _context = context;
            _answer = answer;
        }
        public async Task Create(FormDTO dto, CancellationToken cancellationToken)
        {
            Form newForm = new Form()
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                TemplateId = dto.TemplateId,
                FilledAt = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds)
            };
            await _context.Forms.AddAsync(newForm);
            await _context.SaveChangesAsync(cancellationToken);

            await _answer.Create(dto.Answers, newForm.Id, cancellationToken);
        }
    }
}

using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

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
            bool formExists = await _context.Forms.Where(x => x.TemplateId == dto.TemplateId && x.UserId == dto.UserId).AnyAsync();
            if (formExists)
                return;

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
        public async Task<ICollection<FormDTO>> GetAllByUserId(Guid userId)
        {
            List<Form> forms = await _context.Forms.Where(x => x.UserId == userId).ToListAsync();

            ICollection<FormDTO> dtos = new List<FormDTO>();
            foreach (Form form in forms)
            {
                FormDTO dto = new FormDTO()
                {
                    Id = form.Id,
                    TemplateId = form.TemplateId,
                    FilledAt = DateTime.UnixEpoch.AddSeconds(form.FilledAt).ToLocalTime(),
                };
                dtos.Add(dto);
            }
            return dtos.ToList();
        }

        public async Task<FormDTO> GetById(Guid formId)
        {
            FormDTO dtos = await _context.Forms.Where(x => x.Id == formId).Select(f => new FormDTO
            {
                TemplateId = f.TemplateId,
                FilledAt = DateTime.UnixEpoch.AddSeconds(f.FilledAt).ToLocalTime(),
                Answers = f.Answers.Select(a => new AnswerDTO
                {
                    QuestionId = a.QuestionId,
                    Response = a.Response
                }).ToList()
            }).FirstOrDefaultAsync();

            return dtos;
        }
    }
}

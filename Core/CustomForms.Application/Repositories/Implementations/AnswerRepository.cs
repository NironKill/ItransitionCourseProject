using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IApplicationDbContext _context;

        public AnswerRepository(IApplicationDbContext context) => _context = context;

        public async Task Create(ICollection<AnswerDTO> dtos, Guid formId, CancellationToken cancellationToken)
        {
            List<Answer> answers = new List<Answer>();
            foreach (AnswerDTO dto in dtos)
            {
                Answer newAnswer = new Answer
                {
                    Id = Guid.NewGuid(),
                    FormId = formId,
                    QuestionId = dto.QuestionId,
                    Response = dto.Response,
                };
                answers.Add(newAnswer);
            }
            await _context.Answers.AddRangeAsync(answers, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

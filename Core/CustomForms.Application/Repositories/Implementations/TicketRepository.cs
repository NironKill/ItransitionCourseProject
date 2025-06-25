using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _user;

        public TicketRepository(IApplicationDbContext context, IUserRepository user)
        {
            _context = context;
            _user = user;
        }

        public async Task Create(TicketDTO dto, CancellationToken cancellationToken)
        {
            Ticket ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                TicketUrl = dto.TicketUrl,
                Key = dto.Key,
                TicketJiraId = dto.TicketJiraId,
                AccountId = dto.AccountId
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<ICollection<TicketDTO>> GetAllByUserEmail(string email)
        {
            UserDTO user = await _user.Get(x => x.Email == email);

            List<Ticket> tickets = await _context.Tickets.Where(x => x.UserId == user.Id).ToListAsync();

            ICollection<TicketDTO> dtos = new List<TicketDTO>();
            foreach (Ticket ticket in tickets)
            {
                TicketDTO dto = new TicketDTO()
                {
                    TicketUrl = ticket.TicketUrl,
                    UserId = ticket.UserId,
                    Key = ticket.Key,
                    TicketJiraId = ticket.TicketJiraId,
                    AccountId = ticket.AccountId
                };
                dtos.Add(dto);
            }
            return dtos.ToList();
        }
        public async Task<string> GetAccountId(Guid userId) => await _context.Tickets.Where(x => x.UserId == userId).Select(x => x.AccountId).FirstOrDefaultAsync();
    }
}

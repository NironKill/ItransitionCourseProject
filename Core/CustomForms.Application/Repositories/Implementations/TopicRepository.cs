using CustomForms.Application.Common.Enums;
using CustomForms.Application.DTOs;
using CustomForms.Application.Interfaces;
using CustomForms.Application.Repositories.Interfaces;
using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Repositories.Implementations
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IApplicationDbContext _context;

        public TopicRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create()
        {
            Dictionary<int, string> topics = Enum.GetValues(typeof(Topics)).Cast<Topics>().ToDictionary(t => (int)t, t => t.ToString());

            List<int> listId = await _context.Topics.Select(x => x.Id).ToListAsync();
            foreach (KeyValuePair<int, string> topic in topics)
            {
                if (!listId.Contains(topic.Key))
                {
                    Topic newTopic = new Topic()
                    {
                        Id = topic.Key,
                        Name = topic.Value
                    };
                    await _context.Topics.AddAsync(newTopic);
                    await _context.SaveChangesAsync(default);
                }
            }
        }

        public async Task<ICollection<TopicDTO>> GetAll()
        {
            List<Topic> topics = await _context.Topics.ToListAsync();

            List<TopicDTO> dtos = new List<TopicDTO>();
            foreach (Topic topic in topics)
            {
                TopicDTO dto = new TopicDTO()
                {
                    Id = topic.Id,
                    Name = topic.Name
                };
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}

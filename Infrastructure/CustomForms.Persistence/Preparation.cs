using CustomForms.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Persistence
{
    public static class Preparation
    {
        public static async Task Initialize(ApplicationDbContext context, ITopicRepository repository)
        {
            await context.Database.MigrateAsync();

            await repository.Create();
        }
    }
}

using CustomForms.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomForms.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Form> Forms { get; set; }
        DbSet<Like> Likes { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Template> Templates { get; set; }
        DbSet<Topic> Topics { get; set; }
        DbSet<TemplateTag> TemplateTags { get; set; }
        DbSet<Option> Options { get; set; }
        DbSet<Ticket> Tickets { get; set; }

        DbSet<User> Users { get; set; }
        DbSet<IdentityUserToken<Guid>> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

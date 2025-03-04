using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomForms.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.Templates).WithOne(t => t.User).HasForeignKey(t => t.UserId);

            builder.HasMany(u => u.Likes).WithOne(l => l.User).HasForeignKey(l => l.UserId);

            builder.HasMany(u => u.Forms).WithOne(f => f.User).HasForeignKey(f => f.UserId);

            builder.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.Tickets).WithOne(t => t.User).HasForeignKey(t => t.UserId);

            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}

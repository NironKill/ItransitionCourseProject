using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomForms.Persistence.EntityTypeConfigurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasMany(t => t.Templates).WithOne(q => q.Topic).HasForeignKey(q => q.TopicId);

            builder.Property(t => t.Id).ValueGeneratedNever();
        }
    }
}

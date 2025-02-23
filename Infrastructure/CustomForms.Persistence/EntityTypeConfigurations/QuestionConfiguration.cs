using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomForms.Persistence.EntityTypeConfigurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasMany(q => q.Answers).WithOne(a => a.Question).HasForeignKey(a => a.QuestionId);

            builder.HasMany(q => q.Options).WithOne(o => o.Question).HasForeignKey(o => o.QuestionId);
        }
    }
}

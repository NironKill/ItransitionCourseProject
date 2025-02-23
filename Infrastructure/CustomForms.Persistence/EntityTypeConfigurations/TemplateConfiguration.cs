using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomForms.Persistence.EntityTypeConfigurations
{
    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasMany(t => t.Questions).WithOne(q => q.Template).HasForeignKey(q => q.TemplateId);

            builder.HasMany(t => t.Comments).WithOne(c => c.Template).HasForeignKey(c => c.TemplateId);

            builder.HasMany(t => t.Likes).WithOne(l => l.Template).HasForeignKey(l => l.TemplateId);

            builder.HasMany(t => t.Forms).WithOne(l => l.Template).HasForeignKey(l => l.TemplateId);

            builder.HasMany(t => t.Tags).WithMany(t => t.Templates).UsingEntity<TemplateTag>(
                x => x.HasOne<Tag>(tt => tt.Tag).WithMany(t => t.TemplateTags).HasForeignKey(tt => tt.TagId),
                x => x.HasOne<Template>(tt => tt.Template).WithMany(t => t.TemplateTags).HasForeignKey(tt => tt.TemplateId));
        }
    }
}

using CustomForms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomForms.Persistence.EntityTypeConfigurations
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder) => builder
            .HasMany(f => f.Answers)
            .WithOne(a => a.Form)
            .HasForeignKey(a => a.FormId);
    }
}

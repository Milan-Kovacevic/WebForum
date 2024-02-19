using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Infrastructure.Configuration;

public class TopicEntityConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable(Database.Tables.Topic);
        builder.HasKey(x => x.TopicId);
        builder.HasIndex(x => x.TopicId).IsUnique(true);

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(Database.Constants.MaxNameLength);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(Database.Constants.MaxLongTextLength);
    }
}
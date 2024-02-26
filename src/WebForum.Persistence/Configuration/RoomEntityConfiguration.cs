using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class RoomEntityConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable(Database.Tables.Room);
        builder.HasKey(x => x.RoomId);
        builder.HasIndex(x => x.RoomId).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Database.Constants.MaxNameLength);
        builder.HasIndex(x => x.Name).IsUnique();
        
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(Database.Constants.MaxLongTextLength);
    }
}
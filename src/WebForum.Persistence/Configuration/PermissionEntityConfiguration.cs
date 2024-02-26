using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Persistence.Configuration;

public class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(Database.Tables.Permission);
        builder.HasKey(x => x.PermissionId);
        builder.HasIndex(x => x.PermissionId).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Database.Constants.MaxNameLength);
        builder.HasIndex(x => x.Name).IsUnique();

        var permissions = Enum.GetValues<RoomPermission>()
            .Select(permission => new Permission() { Name = permission.ToString(), PermissionId = (int)permission + 1 });
        builder.HasData(permissions);
    }
}
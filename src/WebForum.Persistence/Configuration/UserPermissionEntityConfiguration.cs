using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class UserPermissionEntityConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable(Database.Tables.UserPermission);
        builder.HasKey(x => new
        {
            x.UserId, x.RoomId, x.PermissionId
        });
        builder.HasIndex(x => new
        {
            x.UserId, x.RoomId, x.PermissionId
        }).IsUnique();
    }
}
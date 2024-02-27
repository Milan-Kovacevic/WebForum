using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Persistence.Configuration;

public class RoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(Database.Tables.Role);
        builder.HasKey(x => x.RoleId);
        builder.HasIndex(x => x.RoleId).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Database.Constants.MaxNameLength);
        builder.HasIndex(x => x.Name).IsUnique();

        var roles = Enum.GetValues<Domain.Enums.UserRole>()
            .Select(role => new UserRole() { Name = role.ToString(), RoleId = (int)role });
        builder.HasData(roles);
    }
}
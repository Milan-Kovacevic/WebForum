using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Application.Abstractions.Services;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class UserEntityConfiguration() : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(Database.Tables.User);
        builder.HasKey(x => x.UserId);
        builder.HasIndex(x => x.UserId).IsUnique();

        builder.Property(x => x.Username)
            .HasMaxLength(Database.Constants.MaxNameLength);
        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.Email).HasMaxLength(Database.Constants.MaxEmailLength);
        builder.Property(x => x.PasswordHash).HasMaxLength(Database.Constants.MaxLongTextLength);
        builder.Property(x => x.DisplayName).HasMaxLength(Database.Constants.MaxNameLength);
        builder.Property(x => x.IsEnabled).IsRequired().HasDefaultValue(Database.Defaults.IsUserEnabled);
        builder.Property(x => x.AccessFailedCount).IsRequired().HasDefaultValue(Database.Defaults.UserAccessFailedCount);
        builder.Property(x => x.LockoutEnd);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(Database.Tables.UserLogin);
        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
        builder.HasIndex(x => new { x.LoginProvider, x.ProviderKey }).IsUnique();
    }
}
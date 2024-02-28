using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(Database.Tables.UserToken);
        builder.HasKey(x => x.TokenId);
        builder.HasIndex(x => x.TokenId).IsUnique();

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Value).IsRequired();
    }
}
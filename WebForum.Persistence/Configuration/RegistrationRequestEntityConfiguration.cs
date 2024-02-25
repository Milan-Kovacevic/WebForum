using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class RegistrationRequestEntityConfiguration : IEntityTypeConfiguration<RegistrationRequest>
{
    public void Configure(EntityTypeBuilder<RegistrationRequest> builder)
    {
        builder.ToTable(Database.Tables.RegistrationRequest);
        builder.HasKey(x => x.RequestId);
        builder.HasIndex(x => x.RequestId).IsUnique();
        builder.Property(x => x.SubmitDate).IsRequired();
    }
}
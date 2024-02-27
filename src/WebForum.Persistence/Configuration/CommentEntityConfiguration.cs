using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForum.Domain.Entities;

namespace WebForum.Persistence.Configuration;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(Database.Tables.Comment);
        builder.HasKey(x => x.CommentId);
        builder.HasIndex(x => x.CommentId).IsUnique();

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(Database.Constants.MaxLongTextLength);
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.DateUpdated);
        builder.Property(x => x.DatePosted);
        builder.Property(x => x.Status).HasConversion<string>();
        
        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
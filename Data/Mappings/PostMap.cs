using JKNews.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JKNews.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");

        builder.HasIndex(x => x.Id);

        /*builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();*/

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Image)
            .HasColumnName("Image")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);

        builder.Property(x => x.Desc)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(1600);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60);

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60);

        builder.HasIndex(x => x.Slug, "IX_Post_Slug")
            .IsUnique();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                tag => tag
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_PostTag_TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                user => user
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostTag_PostId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
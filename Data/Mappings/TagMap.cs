using JKNews.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JKNews.Data.Mappings;

public class TagMap : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tag");

        builder.HasKey(x => x.Id);

        /*builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();*/

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Desc)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(1600);

        builder.HasIndex(x => x.Slug, "IX_Tag_Slug")
            .IsUnique();
    }
}
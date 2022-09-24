using JKNews.Data.Mappings;
using JKNews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JKNews.Data;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new PostMap());
        modelBuilder.ApplyConfiguration(new TagMap());
        
        // Seed Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Name = "Editor", NormalizedName = "Editor".ToUpper() });
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });

        base.OnModelCreating(modelBuilder);
    }
}
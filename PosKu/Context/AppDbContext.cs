using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PosKu.Models;

namespace PosKu.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set max length for UserName, Email, RoleName to avoid exceeding key length
            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(256);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasMaxLength(256);

            modelBuilder.Entity<Role>()
                .Property(r => r.NormalizedName)
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(u => u.NormalizedUserName)
                .HasMaxLength(256);

            // Relationship: User -> Role with Restrict Delete Behavior to avoid cascade delete
            modelBuilder.Entity<User>()
                .HasOne(u => u.RoleDetails)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Relationship: Role -> Users with Restrict Delete Behavior to avoid cascade delete
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.RoleDetails)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Relationship: Role and Menu with many-to-many relationship
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.Roles)
                .WithMany(r => r.Menus)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleMenu",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Menu>().WithMany().HasForeignKey("MenuId").OnDelete(DeleteBehavior.Restrict)
                );
        }
    }
}

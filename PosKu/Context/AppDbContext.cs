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

        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<RoleMenu> RoleMenus { get; set; } = null!; // Add DbSet for RoleMenu

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

            // Relationship: Role and Menu with many-to-many relationship using RoleMenu
            modelBuilder.Entity<RoleMenu>()
                .HasKey(rm => new { rm.RoleId, rm.MenuId });  // Defining composite key

            modelBuilder.Entity<RoleMenu>()
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RoleMenus)  // This links Role to RoleMenu
                .HasForeignKey(rm => rm.RoleId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            modelBuilder.Entity<RoleMenu>()
                .HasOne(rm => rm.Menu)
                .WithMany(m => m.RoleMenus)  // This links Menu to RoleMenu
                .HasForeignKey(rm => rm.MenuId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete
        }
    }    
}

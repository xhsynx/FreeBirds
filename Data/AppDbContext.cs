using Microsoft.EntityFrameworkCore;
using FreeBirds.Models;

namespace FreeBirds.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserRole primary key
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Configure UserRole relationships
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure RefreshToken relationships
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User", Description = "Regular user with limited access" },
                new Role { Id = 2, Name = "Admin", Description = "Administrator with full access" }
            );
        }

        public async Task SeedDatabaseAsync()
        {
            // Check if roles exist
            if (!await Roles.AnyAsync())
            {
                await Roles.AddRangeAsync(
                    new Role { Id = 1, Name = "User", Description = "Regular user with limited access" },
                    new Role { Id = 2, Name = "Admin", Description = "Administrator with full access" }
                );
                await SaveChangesAsync();
            }

            // Check if admin user exists by role
            var adminRole = await Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole != null)
            {
                var adminUser = await Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == adminRole.Id));

                if (adminUser == null)
                {
                    // Create admin user
                    var admin = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Email = "admin@freebirds.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        FirstName = "Admin",
                        LastName = "User",
                        PhoneNumber = "1234567890",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        FailedLoginAttempts = 0
                    };

                    Users.Add(admin);
                    await SaveChangesAsync();

                    // Assign admin role
                    UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });
                    await SaveChangesAsync();
                }
            }

            // Check if regular user exists by role
            var userRole = await Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole != null)
            {
                var regularUser = await Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == userRole.Id));

                if (regularUser == null)
                {
                    // Create regular user
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user",
                        Email = "user@freebirds.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        FirstName = "Regular",
                        LastName = "User",
                        PhoneNumber = "0987654321",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        FailedLoginAttempts = 0
                    };

                    Users.Add(user);
                    await SaveChangesAsync();

                    // Assign user role
                    UserRoles.Add(new UserRole { UserId = user.Id, RoleId = userRole.Id });
                    await SaveChangesAsync();
                }
            }
        }
    }
}
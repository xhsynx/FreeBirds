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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue(UserRole.RegularUser);
        }
    }
}
using Ebay_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Ebay_project.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(k => k.Items)
                .WithOne(b => b.User)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            User user = new User()
            {
                Id = 1,
                Name = "George",
                Password = "Uhorka",
                Role = "Admin"
            };
            modelBuilder.Entity<User>().HasData(user);
        }
    }
}

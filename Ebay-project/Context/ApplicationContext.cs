using Ebay_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Ebay_project.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Bid> Bids { get; set; }

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

            modelBuilder.Entity<Item>()
                .HasMany(k => k.Bids)
                .WithOne(b => b.Item)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(k => k.Bids)
                .WithOne(b => b.User)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

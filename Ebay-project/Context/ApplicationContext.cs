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
            modelBuilder.Entity<Item>()
                .HasOne(u => u.User)
                .WithOne(k => k.Item)
                .HasForeignKey<User>(k => k.ItemId)
                .IsRequired(true);
        }
    }
}

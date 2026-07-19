using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Models;
using System.Diagnostics.Contracts;
namespace MVCDotnetCore.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(e => e.UserId)
                .IsUnique();

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EmailOtp> EmailOtps { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}

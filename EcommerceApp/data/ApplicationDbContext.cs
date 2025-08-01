using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;
using System.Reflection.Emit;

namespace EcommerceApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<PaymentInfo> PaymentInfos => Set<PaymentInfo>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            
            mb.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            
            mb.Entity<WishlistItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<WishlistItem>()
                .HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            
            mb.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

           
            mb.Entity<PaymentInfo>()
                .HasOne(pi => pi.Order)
                .WithOne(o => o.PaymentInfo)
                .HasForeignKey<PaymentInfo>(pi => pi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            
            mb.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            mb.Entity<Order>()
                .Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

            mb.Entity<User>()
    .HasMany(u => u.CartItems)
    .WithOne(c => c.User)
    .HasForeignKey(c => c.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<User>()
                .HasMany(u => u.WishlistItems)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
